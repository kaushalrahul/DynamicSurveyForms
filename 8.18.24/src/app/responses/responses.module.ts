import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResponsesRoutingModule } from './responses-routing.module';
import { EntriesListComponent } from './entries-list/entries-list.component';

import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatTableModule} from '@angular/material/table';
import { EntriesListOverviewComponent } from './entries-list-overview/entries-list-overview.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import {MatPaginatorModule} from '@angular/material/paginator';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    EntriesListComponent,
    EntriesListOverviewComponent
  ],
  imports: [
    CommonModule,
    ResponsesRoutingModule,
    MatCheckboxModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatSnackBarModule,
    MatDialogModule,
    MatPaginatorModule,
    ReactiveFormsModule
  ]
})
export class ResponsesModule { }
