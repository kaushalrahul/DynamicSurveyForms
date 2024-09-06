import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ServiceService } from '../../service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
 
interface Question {
  id: number;
  question: string;
  answerOptions: Array<{ optionValue: string; answerTypeId: number; nextQuestionId?: number }>;
  responseType: string;
  required: boolean; // Ensure this property is included
  sectionId?: string; // Optional property to track the section ID
  isDynamicallyLoaded?: boolean; // Optional property to track dynamically loaded questions
}
 
@Component({
  selector: 'app-conditional-form',
  standalone: true,
  templateUrl: './conditional-form.component.html',
  styleUrls: ['./conditional-form.component.css'],
  imports: [CommonModule, FormsModule, ReactiveFormsModule]
})
export class ConditionalFormComponent implements OnInit, OnDestroy {
  formData: any;
  answerTypes: any[] = [];
  formId: any;
  selectedAnswers: { [sectionId: string]: { [questionId: string]: any } } = {};
  userEmail: string | null = null;
  responseString: string = '';
 
  selectedOptions: any[] = [];
 
  loadedQuestions: { [questionId: number]: Question } = {};
 
  constructor(private formService: ServiceService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private cdr: ChangeDetectorRef,
            private toastr : ToastrService) {}
 
  ngOnInit(): void {
    this.formId = this.activatedRoute.snapshot.queryParams['formId'];
    this.loadAnswerTypes()
      .then(() => {
        this.loadFormData(); 
      })
      .catch(error => {
        console.error('Failed to load answer types:', error);
      });
  }
 
  ngOnDestroy(): void {
  }
 
  loadAnswerTypes(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.formService.getAnswerType().subscribe((res: any) => {
        this.answerTypes = res.data; 
        console.log('Loaded Answer Types:', this.answerTypes); 
        resolve(this.answerTypes);
      }, (error) => {
        console.error('Error loading answer types:', error);
        reject(error);
      });
    });
  }
 
  loadFormData() {
    this.formService.getFormDetailsById(this.formId).subscribe({
      next: (res) => {
        console.log('API Response:', res);
        this.formData = res.form;
        console.log('Form Data after loading:', this.formData);
      },
      error: (err) => {
        alert('Something went wrong in fetching form data');
      }
    });
  }
 
  getResponseTypeId(responseType: string): number {
    console.log('Getting response type ID for:', responseType);
    switch (responseType) {
      case 'text':
        return 1;
      case 'radio':
        return 2;
      case 'checkbox':
        return 3;
      case 'date':
        return 4;
      case 'number':
        return 5;
      case 'multi-select':
        return 6;
      default:
        console.warn('Unknown response type:', responseType); 
        return -1;
    }
  }
 
  loadNextQuestion(nextQuestionId: number, section: any, parentQuestionId: number) {
    this.formService.getNextQuestion(nextQuestionId).subscribe(response => {
      if (response && response.question) {
        const answerTypeId = this.getResponseTypeId(response.responseType);
        console.log('Answer Type ID for new question:', answerTypeId);
 
        const newQuestion: Question = {
          id: response.id,
          question: response.question,
          answerOptions: response.answerOptions.map(option => ({
            ...option,
            answerTypeId: answerTypeId 
          })),
          responseType: response.responseType, 
          required: response.required 
        };
 
      
        this.loadedQuestions[parentQuestionId] = newQuestion;
 
      
        const parentIndex = section.questions.findIndex((q: { id: number }) => q.id === parentQuestionId);
        if (parentIndex !== -1) {
        
          section.questions.splice(parentIndex + 1, 0, newQuestion);
          console.log('New Question Loaded:', newQuestion);
          console.log('Updated Questions:', section.questions);
          this.cdr.detectChanges(); 
        } else {
          console.warn('Parent Question with ID:', parentQuestionId, 'not found.');
        }
      } else {
        console.warn('No question found in response:', response);
      }
    });
  }
 
  getAnswerTypeName(answerTypeId: number | null): string {
    console.log('Getting answer type name for ID:', answerTypeId);
    if (answerTypeId === null || answerTypeId === undefined) {
      console.warn('answerTypeId is null or undefined'); 
      return 'Unknown'; 
    }
 
    switch (answerTypeId) {
      case 1:
        return 'text';
      case 2:
        return 'radio';
      case 3:
        return 'checkbox';
      case 4:
        return 'date';
      case 5:
        return 'number';
      case 6:
        return 'multi-select';
      default:
        console.warn(`No answer type found for ID: ${answerTypeId}`);
        return 'Unknown'; 
    }
  }
 
  isSelected(optionValue: string, question: any): boolean {
    const selectedValues = this.selectedAnswers[question.sectionId]?.[question.id];
    return selectedValues ? selectedValues.includes(optionValue) : false;
  }
 
  handleOptionSelect(event: any, option: any, section: any, question: any) {
    const questionId = question.id; 
    const originalSectionId = section.originalId || section.id; 
 
   
    if (!this.selectedAnswers[originalSectionId]) {
      this.selectedAnswers[originalSectionId] = {};
    }
 
  
    this.selectedAnswers[originalSectionId][questionId] = event.target.value;
    console.log(`Input for question ${questionId}:`, event.target.value); 
    console.log('Updated Selected Answers:', this.selectedAnswers);
 
  
    if (option && option.nextQuestionId) {
      if (event.target.type === 'radio') {
      
        this.removeLoadedQuestion(questionId, section);
      
        this.loadNextQuestion(option.nextQuestionId, section, questionId);
      } else if (event.target.checked) {
        this.loadNextQuestion(option.nextQuestionId, section, questionId);
      } else {
        this.removeLoadedQuestion(questionId, section);
      }
    }
  }
 
  removeLoadedQuestion(parentQuestionId: number, section: any) {
   
    const loadedQuestion = this.loadedQuestions[parentQuestionId];
    if (loadedQuestion) {
      const index = section.questions.findIndex((q: { id: number }) => q.id === loadedQuestion.id);
      if (index !== -1) {
        section.questions.splice(index, 1);
        console.log('Removed loaded question:', loadedQuestion);
        delete this.loadedQuestions[parentQuestionId]; 
        this.cdr.detectChanges(); 
      }
    }
  }
 
  handleMultiSelect(event: any, question: Question, section: any) {
    const selectedOptions = event.target.selectedOptions as HTMLOptionElement[];
    const selectedValues = Array.from(selectedOptions).map(option => option.value);
    const questionId = question.id;
 
    const originalSectionId = section.originalId || section.id;
    if (!this.selectedAnswers[originalSectionId]) {
      this.selectedAnswers[originalSectionId] = {};
    }
  
    this.selectedAnswers[originalSectionId][questionId] = selectedValues;
    console.log(`Multi-select for question ${questionId}:`, selectedValues);
 
   
    selectedValues.forEach(value => {
      const selectedOption = question.answerOptions.find((opt: { optionValue: string }) => opt.optionValue === value);
      if (selectedOption && selectedOption.nextQuestionId) {
        this.loadNextQuestion(selectedOption.nextQuestionId, section, questionId);
      }
    });
  }
 
 
  formIsValid!: boolean;
  formSubmitted = false;
  constraintsIsValid! : boolean;
  formSubmittedConstraints  = false;

 
 
  submitForm(event: any) {
    event.preventDefault();
 
    
    this.formIsValid = this.checkFormValidity();
 
    if (!this.formIsValid) {
      this.formSubmitted = true;
      return;
    }
    
    for (const sectionId in this.selectedAnswers) {
      if (this.selectedAnswers.hasOwnProperty(sectionId)) {
        for (const questionId in this.selectedAnswers[sectionId]) {
          if (this.selectedAnswers[sectionId].hasOwnProperty(questionId)) {
            const question = this.getQuestionById(parseInt(questionId));
            if (question && question.required) {
              const answer = this.selectedAnswers[sectionId][questionId];
              if (!answer || (Array.isArray(answer) && answer.length === 0)) {
                alert(`Please answer the required question: "${question.question}"`);
                return; 
              }
            }
          }
        }
      }
    }
 
    console.log('Final Selected Answers:', this.selectedAnswers);
 
   
    const userEmail = this.formData.email || 'anonymous@example.com'; 
 
    const responseData = {
      id: 0,  
      formID: this.formId,
      email: userEmail,
      response: JSON.stringify(this.selectedAnswers) 
    };
 
    console.log('Response Data to be sent:', responseData);
    
   
    this.formService.addResponse(responseData).subscribe(
      response => {
        console.log('Response from server:', response);
        this.router.navigate(['/thank-you']); 
      },
      error => console.error('Error:', error)
    );
  }
 

  constraintValid: { [key: string]: boolean } = {};

resetConstraintValid() {
  this.constraintValid = {};
}

handleConstraints(event: any, section: any, question: any) {
  const questionId = question.id;
  const answerType = this.getAnswerTypeName(question.answerOptions[0].answerTypeId);
  let constraintValids = false;


  switch (answerType) {
    case 'text':
      const textValue = event.target.value;
      const textConstraint = question.constraint;
      const textConstraintValue = question.constraintValue;

      if (textConstraint === "max") {
        if (textValue.length > textConstraintValue) {
          constraintValids = true;
          this.formSubmittedConstraints = true;
          this.constraintsIsValid = false;
          event.target.value = textValue.substring(0, textConstraintValue);
        }
      } 
      else if (textConstraint === "min") {
        if (textValue.length < textConstraintValue) {
          constraintValids = true;
          this.formSubmittedConstraints = true;
          this.constraintsIsValid = false;
          event.target.value = textValue.substring(0, textConstraintValue);
        }
      } 
      else if (textConstraint === "pattern") {
        const pattern = new RegExp(textConstraintValue, 'i');

        if (!pattern.test(textValue)) {
          constraintValids = true;
          this.formSubmittedConstraints = true;
          this.constraintsIsValid = false;
        }
      }
      break;

    case 'number':
      const numberValue = event.target.valueAsNumber;
      const numberConstraint = question.constraint;
      const numberConstraintValue = question.constraintValue;

      if (numberConstraint === "max") {
        if (numberValue > numberConstraintValue) {
          constraintValids = true;
          this.formSubmittedConstraints = true;
          this.constraintsIsValid = false;
        }
      } 
      else if (numberConstraint === "min") {
        if (numberValue < numberConstraintValue) {
          constraintValids = true;
          this.formSubmittedConstraints = true;
          this.constraintsIsValid = false;
        }
      }
      break;

    default:
      constraintValids = false;
      this.constraintsIsValid = true;
  }

    this.constraintValid[section.id + '_' + question.id] = constraintValids;
    console.log( this.constraintValid[section.id + '_' + question.id] );

    this.constraintsIsValid = Object.values(this.constraintValid).every(valid => valid);

    if (constraintValids) {
      this.toastr.warning(`Constraint validation failed for question: ${question.warningMessage}`);
    }
}

  

 
  checkFormValidity(): boolean {
    for (const section of this.formData.sections) {
      for (const question of section.questions) {
        if (question.required) {
          const sectionAnswers = this.selectedAnswers[section.id];
          const answer = sectionAnswers && sectionAnswers[question.id];
 
          if (!sectionAnswers || !answer) {
           
            this.toastr.warning('No answer found for required question');
            return false;
          }
 
          switch (this.getAnswerTypeName(question.answerOptions[0].answerTypeId)) {
            case 'text':
              if (!answer || answer.trim() === '' ) {
               
                this.toastr.warning('Text answer is empty');
                return false;
 
              }
              break;
 
            case 'number':
              if (!answer || isNaN(answer)) {
               
                this.toastr.warning('Number answer is invalid');
                return false;
              }
              break;
 
            case 'date':
              if (!answer ) {
               
                this.toastr.warning('Date answer is invalid');
                return false;
              }
              break;
 
            case 'radio':
              if (!answer) {
               
                this.toastr.warning('Radio answer is empty');
                return false;
              }
              break;
 
            case 'dropdown':
              if (!answer) {
               
                this.toastr.warning('Dropdown answer is empty');
                return false;
              }
              break;        
 
            case 'checkbox':
            case 'multi-select':
              if (!Array.isArray(answer) || answer.length === 0) {
               
                this.toastr.warning('Checkbox or multi-select answer is empty');
                return false;
              }
              break;
 
            default:
             
              this.toastr.error(`Unknown answer type ID: ${question.answerTypeId}`);
              return false;
          }
        }
      }
    }
    return true;
  }
 
  getQuestionById(questionId: number): Question | null {
    for (const section of this.formData.sections) {
      const question = section.questions.find((q: Question) => q.id === questionId);
      if (question) {
        return question;
      }
    }
    return null;
  }
}
 