import { employeeSidenavLinks } from '../../../shared/constants/employee-sidenav-links';
import { Component, OnInit } from '@angular/core';
import { EmployeeLayoutComponent } from '../employee-layout.component';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AsyncPipe, DatePipe, NgIf } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AppState } from '../../../store';
import * as TimesheetActions from '../../../store/actions/timesheet.actions';
import * as TimesheetSubmitActions from '../../../store/actions/timesheet.submit.actions';
import * as TimesheetSelectors from '../../../store/selectors/timesheet.selectors';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-timesheet-list',
  standalone: true,
  imports: [
    EmployeeLayoutComponent,
    ToolbarComponent,
    MatCardModule,
    MatTableModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    AsyncPipe,
    DatePipe,
    NgIf,
    // NgFor and NgClass removed as not used in template
  ],
  templateUrl: './timesheet-list.component.html',
  styleUrls: ['./timesheet-list.component.scss']
})
export class TimesheetListComponent implements OnInit {
  sidenavLinks = employeeSidenavLinks;
  timesheets$!: Observable<any[]>;
  loading$!: Observable<boolean>;
  currentUser$!: Observable<any>;
  errorMessage$!: Observable<string | null>;
  displayedColumns: string[] = ['date', 'projectCode', 'hours', 'description', 'status'];
  allTimesheets: any[] = [];

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    // Clear error on navigation
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.store.dispatch(TimesheetActions.loadEmployeeTimesheetsFailure({ error: '' }));
      }
    });
    this.timesheets$ = this.store.select(TimesheetSelectors.selectTimesheets);
    this.loading$ = this.store.select(TimesheetSelectors.selectTimesheetLoading);
    this.currentUser$ = this.store.select(AuthSelectors.selectCurrentUser);

    // Load timesheets for current user
    this.currentUser$.subscribe(user => {
      if (user) {
        this.store.dispatch(
          TimesheetActions.loadEmployeeTimesheets({ employeeId: user.id })
        );
      }
    });

    this.timesheets$.subscribe(timesheets => {
      this.allTimesheets = timesheets || [];
      const hasDraft = this.allTimesheets.some((t: any) => t.status === 'Draft');
      this.displayedColumns = ['date', 'projectCode', 'hours', 'description', 'status'];
      if (hasDraft) {
        this.displayedColumns.push('actions');
      }
    });

    // Subscribe to error state
    this.errorMessage$ = this.store.select(TimesheetSelectors.selectTimesheetError);
  }

  /**
   * Create a new timesheet
   */
  createTimesheet(): void {
    this.router.navigate(['/employee/timesheets/new']);
  }

  /**
   * Navigate to edit timesheet form
   */
  editTimesheet(id: number): void {
    this.router.navigate([`/employee/timesheets/${id}/edit`]);
  }

  /**
   * Delete a pending timesheet
   */
  deleteTimesheet(id: number): void {
    if (confirm('Are you sure you want to delete this timesheet?')) {
      this.store.dispatch(TimesheetActions.deleteTimesheet({ id }));
    }
  }

  /**
   * Get status color
   */
  getStatusColor(status: string): string {
    switch (status) {
      case 'Approved':
        return 'primary';
      case 'Rejected':
        return 'warn';
      default:
        return 'accent';
    }
  }

  /**
   * Submit a draft timesheet
   */
  submitTimesheet(id: number): void {
    this.store.dispatch(TimesheetSubmitActions.submitDraftTimesheet({ id }));
  }

  /**
   * Logout the current user
   */
  onLogout(): void {
    // Dispatch logout action or navigate to login
    this.store.dispatch({ type: '[Auth] Logout' });
    this.router.navigate(['/login']);
  }
}
