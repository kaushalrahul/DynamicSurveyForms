import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EditformService } from '../editform.service';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-form-component',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './edit-form-component.component.html',
  styleUrl: './edit-form-component.component.css'
})
export class EditFormComponentComponent {

  formId: any | null = null;
  editForm: FormGroup;
  sectionsDatas : any=[];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private editservice: EditformService
  ) {
    this.editForm = this.fb.group({
      formName: [''],
      questions: this.fb.array([]) 
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.formId = Number(params.get('id')); 
      if (this.formId) {
        this.loadFormDetails();
      }
    });
  }

loadFormDetails(): void {
  this.editservice.getFormDetailById(this.formId).subscribe(form => {
    if (form) {
      this.sectionsDatas = form.sections.map((section: { questions: any[]; }) => ({
        ...section,
        questions: section.questions.map(question => ({
          questionText: question.questionText,
          answerOptions: question.answerOptions,
          answerType: question.answerOptions.length ? question.answerOptions[0].answerType : '' 
        }))
      }));
    }
  });
}





}
