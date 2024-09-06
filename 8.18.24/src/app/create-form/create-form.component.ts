import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SidebarService } from '../sidebar.service';
import { Question, ServiceService } from '../service.service';
import { switchMap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { ToastrService } from 'ngx-toastr';

import jwt_decode from 'jwt-decode';
import { JwtService } from '../services/jwt.service';


@Component({
  selector: 'app-create-form',
  standalone: true,
  templateUrl: './create-form.component.html',
  styleUrls: ['./create-form.component.css'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule] // Add FormsModule here
})
export class CreateFormComponent implements OnInit {
  mainForm: FormGroup;
  sections: { name: string; sortOrder: number; description: string; selectedQuestions: any[] }[] = [];
  isSidebarCollapsed = false;
  isSubmitting = false;
  isPublish: boolean = false;
  userId:any;
  

  // Properties for modal form
  sectionName = '';
  sortOrder = 0;
  description = '';
  selectedQuestionId: any;
  selectedQuestion: Question | null = null;
  questions: Question[] = [];
  sectionToDeleteIndex: number | null = null; // Track the section index to delete
  forms: any[] = [];
  selectedFormId: number | null = null;
 
  constructor(
    private sidebarService: SidebarService,
    private formService: ServiceService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private toaster:ToastrService,
    private jwtService: JwtService
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => this.isSidebarCollapsed = collapsed
    );

    this.mainForm = new FormGroup({
      formName: new FormControl('', [Validators.required]),
      formDescription: new FormControl(''),
      sections: new FormArray([]),
      
    });
  }
  



  ngOnInit(): void {

    this.userId = this.jwtService.getUserIdFromToken();
    console.log('User ID:', this.userId);

    this.loadQuestions();
    this.loadForms();
  }



  // getUserIdFromToken(): number | null {
  //   const token = localStorage.getItem('authToken');
    
  //   if (token) {
  //     const parts = token.split('.');
  //     if (parts.length !== 3) {
  //       console.error('Invalid token structure');
  //       return null;
  //     }
  //     const payload = parts[1]; 
  //     const decodedPayload = JSON.parse(this.base64UrlDecode(payload));
  //     return decodedPayload.UserID || null;  
  //   }
  //   return null;
  // }
  // private base64UrlDecode(str: string): string {
  //   str = str.replace(/-/g, '+').replace(/_/g, '/');
  //   while (str.length % 4) {
  //     str += '=';
  //   }
  //   return atob(str);  
  // }
  




  loadForms(): void {
    this.formService.getAllForm().subscribe({
      next: (data) => {
        console.log(data);
        this.forms = data;
      },
      error: (err) => {
        console.error('Error fetching forms:', err);
      }
    });
  }

  copyForm(): void {
    if (this.selectedFormId) {
      console.log(`Copying form with ID: ${this.selectedFormId}`);
      
      // Fetch the form details using the selected form ID
      this.formService.getFormDetailsByFormId(this.selectedFormId).subscribe({
        next: (data) => {
          console.log(data);
          // Patch the form metadata
          // this.mainForm.patchValue({
          //   // formName: `Copy of ${data.formName}`, // Modify the name to indicate it's a copy
            
          // });
  
          // Clear existing sections and populate with the copied form's sections
          this.sections = data.sections.map(section => ({
            name: section.sectionName,
            description: 'hello',
            sortOrder: 1,
            // answerType:section.a
            selectedQuestions: section.questions.map(question => ({
              id: question.id,
              question: question.questionText,
              // answerType:answerOptions.answerType,
              answerOptions: question.answerOptions // Assuming answerOptions is part of question
            }))
          }));
  
          console.log('Form copied successfully:', this.mainForm.value, this.sections);
          this.toaster.success("Form copied successfully");
        },
        error: (err) => {
          console.error('Error fetching form details:', err);
          this.toaster.error('Error fetching form details');
        }
      });
    } else {
      console.log('No form selected');
      this.toaster.error('No form selected');
    }
  }

  onSelectChange(event: any) {
    const value = event.target.value;
    this.selectedQuestionId = value;
  }

  get formControls() {
    return this.mainForm.controls;
  }

  loadQuestions() {
    this.formService.getAllQuestions().subscribe(
      (questions) => {
        this.questions = questions;
      },
      (error) => {
        console.error('Error fetching questions:', error);
      }
    );
  }

  addSection() {
    if (this.sectionName && this.sortOrder >= 0 && this.description) {
      const newSection: any = {
        name: this.sectionName,
        sortOrder: this.sortOrder,
        description: this.description,
        selectedQuestions: []
      };
      this.sections.push(newSection);

      this.sectionName = '';
      this.sortOrder = 0;
      this.description = '';
      this.cdr.detectChanges();
    }
  }

  patchedSectionName : any;
  copyPatchedSectionName : any;
  patchedSortOrder! : number;
  patchedDescription! :string;

  patchSectionValue(section : any){
    // let sectionFetched = this.sections.filter((sec : any) => sec.name === section.name);

    const sectionFetched = this.sections.find((sec) => sec.name === section.name);

  // Check if a matching section was found
  if (sectionFetched) {
    this.patchedSectionName = sectionFetched.name;
    this.patchedSortOrder = sectionFetched.sortOrder;
    this.patchedDescription = sectionFetched.description;
  } 

  this.copyPatchedSectionName =  this.patchedSectionName;


  }

  saveValueEdit() {
    const sectionFetched = this.sections.find((sec) => sec.name === this.copyPatchedSectionName);

    if(sectionFetched){
      sectionFetched.name = this.patchedSectionName;
      sectionFetched.sortOrder = this.patchedSortOrder;
      sectionFetched.description = this.patchedDescription;
    }

  }

async publishForm() {
  // Prevent submission if the form is invalid or currently submitting
  if (this.mainForm.invalid || this.isSubmitting) {
    console.log('Form is invalid or submission is in progress');
    // this.toaster.error("Form is invalid or submission is in progress");
    return;
  }

  

  // const userId = this.getUserIdFromToken();
    if (!this.userId) {
      this.toaster.error('User ID not found');
      return;
    }


  this.isSubmitting = true;
  this.isPublish = true; 
  

  // Prepare form data
  const formData = {
    formName: this.formControls['formName'].value,
    description: this.formControls['formDescription'].value,
    userId: this.userId, 
    createdOn: new Date().toISOString(),
    isPublish: this.isPublish, 
    version: 1

  };

  try {
    
    console.log(formData);
    const formId = await this.formService.createForm(formData).toPromise();

    
    if (!formId) {
      throw new Error('Form ID is undefined or invalid');
    }

    
    for (const section of this.sections) {
      const sectionData = {
        formId: Number(formId),
        sectionName: section.name,
        description: section.description,
        slno: 22 
      };

      
      const createdSection = await this.formService.associateSectionsWithForm(sectionData).toPromise();

      
      for (const question of section.selectedQuestions) {
        try {
          const result = await this.formService.MapSectionToQuestion(question.id, createdSection.sectionId).toPromise();
          console.log('MapSectionToQuestion response:', result);
        } catch (error) {
          console.error('Error mapping question to section:', error);
        }
      }
    }

    
    console.log('Form successfully published');
    this.toaster.success("Form successfully published");
    this.router.navigate(['app/dashboard']);
  } catch (error) {
   
    console.error('Error during form submission:', error);
    this.toaster.error("Error during form submission");
  } finally {
   
    this.isSubmitting = false;
  }
}



async saveDraft() {
  if (this.mainForm.invalid || this.isSubmitting) {
    console.log('Form is invalid or submission is in progress');
    
    return;
  }

  this.isSubmitting = true;
  this.isPublish = false; 

 
  const formData = {
    formName: this.formControls['formName'].value,
    description: this.formControls['formDescription'].value,
    userId: 1, 
    createdOn: new Date().toISOString(),
    isPublish: this.isPublish 
  };

  try {
 
    console.log(formData);
    const formId = await this.formService.createForm(formData).toPromise();

    
    if (!formId) {
      throw new Error('Form ID is undefined or invalid');
    }

    
    for (const section of this.sections) {
      const sectionData = {
        formId: Number(formId),
        sectionName: section.name,
        description: section.description,
        slno: 22 // Adjust 'slno' as necessary
      };

      // Create section and associate it with the form
      const createdSection = await this.formService.associateSectionsWithForm(sectionData).toPromise();

      // Map each question to the corresponding section
      for (const question of section.selectedQuestions) {
        try {
          const result = await this.formService.MapSectionToQuestion(question.id, createdSection.sectionId).toPromise();
          console.log('MapSectionToQuestion response:', result);
        } catch (error) {
          console.error('Error mapping question to section:', error);
        }
      }
    }

    // Show success message and navigate to the dashboard
    console.log('Form successfully published');
    this.toaster.success("Form successfully saved in Draft");
    this.router.navigate(['app/dashboard']);
  } catch (error) {
    // Handle any errors that occur during form submission
    console.error('Error during form submission:', error);
    this.toaster.error("Error during form submission");
  } finally {
    // Ensure that isSubmitting is reset even if an error occurs
    this.isSubmitting = false;
  }
}


  onexist(index: number): void {
    if (this.selectedQuestionId !== null) {
      this.formService.getQuestionById(this.selectedQuestionId).subscribe(
        (question) => {
          this.selectedQuestion = question;

          const section = this.sections[index];
          const alreadyExists = section.selectedQuestions.some(q => q.id === this.selectedQuestion?.id);
        
          if (!alreadyExists) {
            section.selectedQuestions.push(this.selectedQuestion);
          }

          this.selectedQuestionId = null;
        },
        (error) => {
          console.error('Error fetching question details:', error);
        }
      );
    } else {
      console.log('No question selected.');
    }
  }

  editQuestion(questionId: number) {
    this.router.navigate([`app/edit-question/${questionId}`]);
    console.log('Edit question with ID:', questionId);
  }
  
  
  deleteQuestion(questionId: number) {
    console.log('Delete question with ID:', questionId);
    // Find and remove the question from the selectedQuestions of each section
    this.sections.forEach(section => {
      const index = section.selectedQuestions.findIndex(q => q.id === questionId);
      if (index !== -1) {
        section.selectedQuestions.splice(index, 1);
      }
    });
  }

  confirmDeleteSection(index: number) {
    this.sectionToDeleteIndex = index;
  }

  deleteSection() {
    if (this.sectionToDeleteIndex !== null) {
      this.sections.splice(this.sectionToDeleteIndex, 1);
      this.sectionToDeleteIndex = null;
    }
  }



    
  
}


