import { ChangeDetectorRef, Component, Version } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Question, ServiceService } from '../service.service';
import { SidebarService } from '../sidebar.service';
import { CommonModule } from '@angular/common';
import { SectionService } from '../services/section.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './edit-form.component.html',
  styleUrl: './edit-form.component.css'
})
export class EditFormComponent {

  isPublish: boolean = false;
  selectedForm: string = '';
  forms: any[] = []; 
  mainForm: FormGroup;
  sections: {
    sectionId: number; name: string; sortOrder: number; description: string; selectedQuestions: any[] 
}[] = [];  

  isSidebarCollapsed = false;
  isSubmitting = false;
  selectedFormId: number | null = null;

  
  sectionName = '';
  sortOrder = 0;
  description = '';
  selectedQuestionId: any;
  selectedQuestion: Question | null = null;
  questions: Question[] = [];
  sectionToDeleteIndex: number | null = null; 

  constructor(
    private sidebarService: SidebarService,
    private formService: ServiceService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private sectionService: SectionService,
    private toaster:ToastrService
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => this.isSidebarCollapsed = collapsed
    );

    this.mainForm = new FormGroup({
      formName: new FormControl('', [Validators.required]),
      formDescription: new FormControl(''),
      sections: new FormArray([])
    });
  }

  ngOnInit(): void {
    this.loadQuestions();
    this.loadFormDetails();
    this.loadForms();
  }



  onFormSelect(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedForm = selectElement.value;
    
  }

  formDetail : any;
  loadFormDetails() {
    const formId = this.route.snapshot.params['id'];
    this.formService.getFormDetailsByFormId(formId).subscribe(
      (formDetails) => {
        console.log(formDetails)
        this.formDetail = formDetails;
        this.patchFormValues(formDetails);
      },
      (error) => {
        console.error('Error fetching form details:', error);
      }
    );
  }

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

  




  patchFormValues(formDetails: any) {
    this.mainForm.patchValue({
      formName: formDetails.formName,
      formDescription: formDetails.description,
    });
  
    if (formDetails.sections && formDetails.sections.length) {
      this.sections = formDetails.sections.map((section: any) => ({
        sectionId: section.id,
        name: section.sectionName,
        sortOrder: section.sortOrder,
        description: section.description,
        selectedQuestions: section.questions.map((question: any) => ({
          id: question.id,
          question: question.questionText,
           responseType: question.answerOptions[0].answerType,
          
          answerOptions: question.answerOptions.map((option: any) => ({
            id: option.id,
            optionValue: option.optionValue
          }))
        }))
      }));
    }
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

  onSelectChange(event: any) {
    const value = event.target.value;
    this.selectedQuestionId = value;
  }



  get formControls() {
    return this.mainForm.controls;
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



async publishForm() {
  if (this.mainForm.invalid || this.isSubmitting) {
    console.log('Form is invalid or submission is in progress');
    return;
  }

  this.isSubmitting = true;
  this.isPublish = true; 


  const formData = {
    formName: this.formControls['formName'].value,
    description: this.formControls['formDescription'].value,
    userId: 1,
    updatedOn: new Date().toISOString(),
    isPublish: this.isPublish,
    version : this.formDetail.version
   

  };
  console.log(formData);
  try {
   
    const formId = this.route.snapshot.params['id'];
    
    if (!formId) {
      throw new Error('Form ID is undefined or invalid');
    }
    
    
    await this.formService.updateForm(formId, formData).toPromise();
    console.log('Form updated with ID:', formId);

    
    for (const section of this.sections) {
      const sectionData = {
        formId: Number(formId),
        sectionName: section.name,
        description: section.description,
        slno: section.sortOrder
      };
      
      let sectionId = section.sectionId;
      
      if (sectionId) {
        await this.sectionService.updateSection(sectionId, sectionData).toPromise();
        console.log('Section updated with ID:', sectionId);
      } else {
        try {
          const createdSection = await this.sectionService.createSection(sectionData).toPromise();
          section.sectionId = createdSection.sectionId;
          console.log('Section created with ID:', section.sectionId);
        } catch (error) {
          console.error('Error creating section:', error);
        }
      }
      

      for (const question of section.selectedQuestions) {
        try {
          console.log(section.sectionId , ":" ,  question.id);

          await this.formService.MapSectionToQuestion(question.id, section.sectionId).toPromise();
          console.log('Question mapped to section:', question.id, section.sectionId);
        } catch (error) {
          console.error('Error mapping question to section:', error);
        }
      }
    }

    console.log('Form successfully updated');
    this.toaster.success("Form published successfully");
    this.router.navigate(['app/dashboard']);
  } catch (error) {
    console.error('Error during form submission:', error);
    this.toaster.error("Error during form submission");
  } finally {
    this.isSubmitting = false;
  }
}

async saveDraft(){
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
    updatedOn: new Date().toISOString()
  };

  try {
    const formId = this.route.snapshot.params['id'];
    
    if (!formId) {
      throw new Error('Form ID is undefined or invalid');
    }
    
    await this.formService.updateForm(formId, formData).toPromise();
    console.log('Form updated with ID:', formId);

    for (const section of this.sections) {
      const sectionData = {
        formId: Number(formId),
        sectionName: section.name,
        description: section.description,
        slno: section.sortOrder
      };
      
      let sectionId = section.sectionId;
      
      if (sectionId) {
        await this.sectionService.updateSection(sectionId, sectionData).toPromise();
        console.log('Section updated with ID:', sectionId);
      } else {
        try {
          const createdSection = await this.sectionService.createSection(sectionData).toPromise();
          section.sectionId = createdSection.sectionId;
          console.log('Section created with ID:', section.sectionId);
        } catch (error) {
          console.error('Error creating section:', error);
        }
      }
      

      for (const question of section.selectedQuestions) {
        try {
          console.log(section.sectionId , ":" ,  question.id);

          await this.formService.MapSectionToQuestion(question.id, section.sectionId).toPromise();
          console.log('Question mapped to section:', question.id, section.sectionId);
        } catch (error) {
          console.error('Error mapping question to section:', error);
        }
      }
    }

    console.log('Form successfully updated');
    this.toaster.success('Form Successfully Updated');
    this.router.navigate(['app/dashboard']);
  } catch (error) {
    console.error('Error during form submission:', error);
  } finally {
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
    console.log('Edit question with ID:', questionId);
    this.router.navigate([`app/edit-question/${questionId}`])
  }

  deleteQuestion(questionId: number, sectionId: number) {
    console.log('Delete question with ID:', questionId, sectionId);
    this.formService.MapSectionToQuestionDelete(sectionId, questionId).subscribe({
      next : (res)=>{
        console.log(res);
      }
    });
    this.sections.forEach(section => {
      const index = section.selectedQuestions.findIndex(q => q.id === questionId);
      if (index !== -1) {
        section.selectedQuestions.splice(index, 1);
      }
    });
  }

  confirmDeleteSection(index: number, sectionId : number) {
    this.sectionToDeleteIndex = index;

    console.log(sectionId );

    this.sectionService.deleteSection(sectionId).subscribe({
      next: (res)=>{
        console.log(res);
      }
    })
  }
  
  
     deleteSection() {
    if (this.sectionToDeleteIndex !== null) {
      this.sections.splice(this.sectionToDeleteIndex, 1);
      this.sectionToDeleteIndex = null;
    }
  }
}
