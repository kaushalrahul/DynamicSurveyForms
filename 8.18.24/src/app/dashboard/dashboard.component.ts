import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SidebarService } from '../sidebar.service';
import { Router } from '@angular/router';
import { FormGroup, FormsModule } from '@angular/forms';  
import { NgxPaginationModule } from 'ngx-pagination';  
import { FilterPipe } from '../filter.pipe';
import { ChangeDetectorRef } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ServiceService } from '../service.service'; 
import { HttpClientModule } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { DeleteConfirmationModalComponent } from '../delete-confirmation-modal/delete-confirmation-modal.component'; // Adjust the path as needed
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [HttpClientModule,CommonModule, FormsModule, NgxPaginationModule, FilterPipe, MatButtonModule, MatIconModule, MatMenuModule]  
})
export class DashboardComponent implements OnInit {
  forms: any[] = [];
  isSidebarCollapsed = false;
  searchQuery: string = '';
  p: number = 1;
  showDropdown: number | null = null; 
  currentLink: string = '';
  
  selectedForms: Set<number> = new Set(); 
  sortColumn: string = 'formName'; 
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(
    private sidebarService: SidebarService, 
    private router: Router, 
    private cdr: ChangeDetectorRef,
    private serviceService: ServiceService, 
    private dialog: MatDialog ,
    private toaster:ToastrService
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => this.isSidebarCollapsed = collapsed
    );
  }

  ngOnInit(): void {
    this.fetchForms();
  }

  fetchForms() {
    this.serviceService.getAllForm().subscribe({
      next: (data) => {
        this.forms = data.map((form: any) => {
          form.isPublished = form.isPublish === 'true' || form.isPublish === true;
          return form;
        });
        this.sortForms(); 
        console.log(this.forms);  
      },
      error: (error) => {
        console.error('Error fetching forms:', error);
      }
    });
  }

  sortForms() {
    this.forms.sort((a, b) => {
      const aValue = a[this.sortColumn];
      const bValue = b[this.sortColumn];
      const direction = this.sortDirection === 'asc' ? 1 : -1;

      if (aValue < bValue) return -direction;
      if (aValue > bValue) return direction;
      return 0;
    });
  }

  setSort(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.sortForms();
  }

  openShareDialog(form: any) {
    this.currentLink = `http://localhost:4200/user/user?formId=${form.id}`;
    document.getElementById('shareModal')!.style.display = 'block';
  }
  openDeleteModal(formId: number): void {
    const dialogRef = this.dialog.open(DeleteConfirmationModalComponent);
  
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.deleteForm(formId);
      }
    });
  }

  copyToClipboard() {
    const copyText = document.getElementById('formLink') as HTMLInputElement;
    copyText.select();
    document.execCommand('copy');
    this.toaster.success("Link copied to clipboard!");
  }


  closeModal() {
    document.getElementById('shareModal')!.style.display = 'none';
  }

  createSurvey() {
    this.router.navigate(['/app/create-form']);
    console.log('Create Survey button clicked');
  }

  response(form: any) {
    console.log("hello");
    this.router.navigate([`/app/responses/entries-list`, form.id]);
  }

  viewForm(form: any) {
    console.log('View form:', form);
    this.router.navigate([`/edit-form`, form.id]);
  }

  editForm(form: any) {
    console.log('Edit form:', form);
    this.router.navigate([`/app/edit-form/${form.id}`]);
  }

  deleteForm(id: number): void {
    if (id === 0) {
      console.error('Invalid form ID: 0');
      alert('Failed to delete the form. Invalid ID.');
      return;
    }
  
    if (confirm('Are you sure you want to delete this form?')) {
      this.serviceService.deleteForm(id).subscribe({
        next: () => {
          this.forms = this.forms.filter(f => f.id !== id);
        },
        error: (error) => {
          console.error('Error deleting form:', error);
          alert('Failed to delete the form. Please try again.');
        }
      });
    }
  }

  deleteSelectedForms() {
    if (confirm('Are you sure you want to delete the selected forms?')) {
      this.selectedForms.forEach(id => {
        this.serviceService.deleteForm(id).subscribe({
          next: () => {
            this.forms = this.forms.filter(f => !this.selectedForms.has(f.id));
          },
          error: (error) => {
            console.error('Error deleting form:', error);
            alert('Failed to delete one or more forms. Please try again.');
          }
        });
      });
      this.selectedForms.clear(); 
    }
  }

  onCheckboxChange(event: Event, formId: number) {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.selectedForms.add(formId);
    } else {
      this.selectedForms.delete(formId);
    }
  }

  toggleSelectAll(event: Event) {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.forms.forEach(form => this.selectedForms.add(form.id));
    } else {
      this.selectedForms.clear();
    }
  }

  isSelected(formId: number): boolean {
    return this.selectedForms.has(formId);
  }

  shareForm(form: any) {
    this.openShareDialog(form);
  }

  toggleDropdown(formId: number) {
    this.showDropdown = this.showDropdown === formId ? null : formId;
    this.cdr.detectChanges(); 
  }

}
