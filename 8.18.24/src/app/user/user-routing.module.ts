import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConditionalFormComponent } from './conditional-form/conditional-form.component';

const routes: Routes = [
  { path: "user", component: ConditionalFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
