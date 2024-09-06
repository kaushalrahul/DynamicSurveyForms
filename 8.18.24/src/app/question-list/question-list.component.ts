import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { QuestionDto } from '../QuestionDto';
import { QuestionService } from '../question.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarService } from '../sidebar.service';
import { DeleteConfirmationModalComponent } from '../delete-confirmation-modal/delete-confirmation-modal.component'; // Adjust the path as needed
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class QuestionListComponent implements OnInit {
  questions: QuestionDto[] = [];
  loading = false;
  error: string | null = null;
  isSidebarCollapsed = false; 

  constructor(
    private questionService: QuestionService,
    private toastr: ToastrService,
    private router: Router,
    private sidebarService: SidebarService,
    private dialog: MatDialog 
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => (this.isSidebarCollapsed = collapsed)
    );
  }

  ngOnInit(): void {
    this.fetchQuestions();
  }

  fetchQuestions(): void {
    this.loading = true;
    this.error = null; 

    this.questionService.getAllQuestions().subscribe({
      next: (data: QuestionDto[]) => {
        this.questions = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load questions';
        this.loading = false;
      }
    });
  }

  editQuestion(id: number): void {
    this.router.navigate(['/app/edit-question', id]);
  }

  deleteQuestion(id: number): void {
    const dialogRef = this.dialog.open(DeleteConfirmationModalComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) { 
        this.questionService.deleteQuestion(id).subscribe({
          next: () => {
            this.toastr.success('Question deleted successfully');
            this.fetchQuestions(); 
          },
          error: () => {
            this.toastr.error('Failed to delete question');
          }
        });
      }
    });
  }
}
