import { Component, NgModule } from '@angular/core';
import { SidebarService } from '../sidebar.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuestionService } from '../question.service';
import { QuestionDto } from '../QuestionDto';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ServiceService } from '../service.service';
import { JwtService } from '../services/jwt.service';

@Component({
  selector: 'app-create-question',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-question.component.html',
  styleUrl: './create-question.component.css'
})
export class CreateQuestionComponent {
  questionName: string = '';
  nextQuestion: string = '';
  nextQuestionOptions: QuestionDto[] = [];

  sortOrder: number | null = null;
  responseType: string = '';
  options: { optionText: string, nextQuestion: string | number | null }[] = [{ optionText: '', nextQuestion: '' }];
  userId:any;
  // Constraint-related properties
  addConstraint: boolean = false;
  requiredQuestionCheck: boolean = false;
  constraintDatatype: string = '';
  constraintType: string = '';
  constraintValue: string = '';
  constraintWarningMessage: string = '';
  isSidebarCollapsed = false;
  isConstraintEnabled = false;
  apiUrl = 'https://localhost:7239/api/Question';

  questionId: string | null = null; // For handling updates
  pageTitle: string = 'Create New Question'; // Dynamic title for creation or update
  buttonLabel: string = 'Create Question'; // Dynamic button label

  constructor(
    private sidebarService: SidebarService,
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService,
    // private formService:ServiceService,
    private toaster: ToastrService,
    private jwtService: JwtService
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => (this.isSidebarCollapsed = collapsed)
    );
  }

  ngOnInit(): void {

    this.userId = this.jwtService.getUserIdFromToken();
    console.log('User ID:', this.userId);
    this.loadAllQuestions();

    this.route.params.subscribe((params: Params) => {
      const id = params['id'];
      if (id) {
        this.questionId = id;
        this.pageTitle = 'Update Question';
        this.buttonLabel = 'Update Question'; 
        this.loadQuestionData(id); 
      }
    });
  }

  updateRequired() {
    this.requiredQuestionCheck = !this.requiredQuestionCheck;
    console.log(this.requiredQuestionCheck);
  }


  loadAllQuestions() {
    this.questionService.getAllQuestions().subscribe({
      next: (questions) => {
        this.nextQuestionOptions = questions;
      },
      error: (error) => {
        console.error('Error fetching questions', error);
      }
    });
  }



  loadQuestionData(id: string) {
    this.http.get<QuestionDto>(`${this.apiUrl}/${id}`).subscribe({
      next: (data) => {
        console.log(data);
        this.questionName = data.question;
        this.sortOrder = data.serialNumber;
        this.responseType = data.responseType;
        this.requiredQuestionCheck = data.required;
        this.options = data.answerOptions.map(option => ({
          optionText: option.optionValue,
          nextQuestion: option.nextQuestionId?.toString() || ''
        }));

        this.constraintDatatype = data.dataType;
        this.constraintType = data.constraint;
        this.constraintValue = data.constraintValue;
        this.constraintWarningMessage = data.warningMessage;

        this.addConstraint = !!(data.dataType || data.constraint || data.constraintValue || data.warningMessage);
        this.isConstraintEnabled = this.addConstraint;
      },
      error: (error) => {
        console.error('Error loading question data', error);
      }
    });
  }

  mapResponseType(responseTypeValue: string): string {
    const responseTypeMapping: { [key: string]: string } = {
      '1': 'text',
      '2': 'radio',
      '3': 'checkbox',
      '4': 'date',
      '5': 'number',
      '6': 'multi-select'
    };
    return responseTypeMapping[responseTypeValue] || '';
  }

  toggleConstraintSection(event: any) {
    this.isConstraintEnabled = event.target.checked;
  }

  onResponseTypeChange() {
    if (this.responseType) {
      this.options = [{ optionText: '', nextQuestion: null }];
    }
  }

  addOption() {
    this.options.push({ optionText: '', nextQuestion: null });
  }

  deleteOption(index: number) {
    this.options.splice(index, 1);
  }

  onSubmit() {

    const authToken = localStorage.getItem('authToken');  // Retrieve the token from localStorage

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${authToken}`,  
      'Content-Type': 'application/json'  
    });


    const responseTypeMapping: { [key: string]: string } = {
      'text': "1",
      'radio': "2",
      'checkbox': "3",
      'date': "4",
      'number': "5",
      'multi-select': "6"
    };

    const responseTypeValue = responseTypeMapping[this.responseType];
    console.log(responseTypeValue);
    if (responseTypeValue === undefined) {
      console.error('Invalid response type');
      return;
    }

    const payload: QuestionDto = {
      question: this.questionName,
      serialNumber: this.sortOrder ?? 0,
      responseType: responseTypeValue,
      answerOptions: this.options.map(option => ({
        optionValue: option.optionText,
        nextQuestionId: option.nextQuestion ? Number(option.nextQuestion) : undefined // Use correct nextQuestionId type
      })),
      dataType: this.constraintDatatype,
      constraint: this.constraintType,
      constraintValue: this.constraintValue,
      warningMessage: this.constraintWarningMessage,
      required: this.requiredQuestionCheck,
      nextQuestionId: '',
      id: 0,
      userId: this.userId,
    };
    


    console.log('Payload being sent:', payload);
    if (this.questionId) {
     
      debugger
      this.http.put(`${this.apiUrl}/${this.questionId}`, payload).subscribe({
        next: (response) => {
          console.log('Question updated successfully', response);
          this.toaster.success("Question Updated Successfully");
          this.router.navigate(['app/dashboard']);
        },
        error: (error) => {
          console.error('Error updating question', error);
          this.toaster.error("Error updating the question");
        }
      });
    } else {
      // Create new question
      this.http.post(`${this.apiUrl}/CreateQuestion`, payload).subscribe({
        next: (response) => {
          console.log('Question created successfully', response);
          this.toaster.success("Question Created Successfully");
          this.router.navigate(['app/dashboard']);
        },
        error: (error) => {
          console.error('Error creating question', error);
          this.toaster.error("Error creating the question");
        }
      });
    }
  }

}
