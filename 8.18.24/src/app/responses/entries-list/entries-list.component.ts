import { Component, OnInit, ViewChild  } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseService } from '../../services/response.service';
import { Entry } from '../../interfaces/entry';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { SidebarService } from '../../sidebar.service';
import { MatSort } from '@angular/material/sort';
//import { SidebarService } from  '/services/sidebar.service';


@Component({
  selector: 'app-entries-list',
  templateUrl: './entries-list.component.html',
  styleUrl: './entries-list.component.css'
})
export class EntriesListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'email', 'actions'];
  dataSource = new MatTableDataSource<Entry>([]);
  searchControl = new FormControl('');
  totalEntries: number = 0;
  formId: number | null = null;
  isSidebarCollapsed = false; 

  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private responseService: ResponseService,
    private router: Router,
    private route: ActivatedRoute,
    private sidebarService: SidebarService
  ) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => (this.isSidebarCollapsed = collapsed)
    );
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('formId');
      if (id) {
        this.formId = +id;
        this.loadEntries();
        this.loadTotalEntries();
        this.setupSearch();
      }
    });
  }

  loadEntries(): void {
    if (this.formId !== null) {
      this.responseService.getResponseIdsByFormId(this.formId).subscribe(responseIds => {
        const entries: Entry[] = [];
        responseIds.forEach(id => {
          this.responseService.getResponseById(id).subscribe(response => {
            const entry: Entry = {
              id: response.id,
              email: response.email,
            };
            entries.push(entry);
            this.dataSource.data = entries;
            this.dataSource.sort = this.sort; // Set the sort instance
            this.totalEntries = entries.length; // Update totalEntries
          });
        });
      });
    }
  }

  loadTotalEntries(): void {
    if (this.formId !== null) {
      this.responseService.getResponseCountByFormId(this.formId).subscribe({
        next: (count) => {
          this.totalEntries = count;
        },
        error: (error) => {
          console.error('Error fetching total entries count:', error);
        }
      });
    }
  }

  setupSearch(): void {
    this.searchControl.valueChanges.subscribe(value => {
      const filterValue = value ? value.trim().toLowerCase() : '';
      this.dataSource.filter = filterValue;
      this.totalEntries = this.dataSource.filteredData.length;
    });
  }

  redirectToOverviewPage(element: Entry): void {
    this.router.navigate(['/app/responses/entries-list-overview', element.id]);
  }
}
