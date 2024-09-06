import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EntriesListComponent } from './entries-list/entries-list.component';
import { EntriesListOverviewComponent } from './entries-list-overview/entries-list-overview.component';

const routes: Routes = [
  {
    path: 'entries-list/:formId',
    component: EntriesListComponent
  },
  {
    path: 'entries-list-overview/:id',
    component: EntriesListOverviewComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResponsesRoutingModule { }
