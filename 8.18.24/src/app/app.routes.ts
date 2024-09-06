import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupComponent } from './signup/signup.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { CreateFormComponent } from './create-form/create-form.component';
import { CreateQuestionComponent } from './create-question/create-question.component';
import { ResponsesModule } from './responses/responses.module';
import { EditFormComponent } from './edit-form/edit-form.component';
import { EditFormComponentComponent } from './edit-form-component/edit-form-component.component';
import { QuestionListComponent } from './question-list/question-list.component';
import { SubmitFormComponent } from './submit-form/submit-form.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/sign-in',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      { path: '', redirectTo: 'sign-in', pathMatch: 'full' },
      { path: 'sign-in', component: SignupComponent },
    ]
  },
  {
    path: 'app',
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'create-form', component: CreateFormComponent },
      { path: "add-question", component: CreateQuestionComponent },
      {
        path: 'edit-form/:id',  
        component: EditFormComponent
      },
      { path: "edit-question/:id", component: CreateQuestionComponent },
      {path: 'responses', loadChildren: ()=>ResponsesModule},
      { path: 'questions-list', component: QuestionListComponent },
      
    ]
  },
  {
    path: 'edit-form/:id', 
    component: EditFormComponentComponent
  },
  { path: 'thank-you', component: SubmitFormComponent},

  {
  path : 'user', loadChildren : () => import('./user/user.module').then(m => m.UserModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
