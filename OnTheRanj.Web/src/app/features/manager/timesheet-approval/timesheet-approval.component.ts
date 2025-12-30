import { Component, OnInit, inject } from '@angular/core';
import { TimesheetApprovalService } from '../../../core/services/timesheet-approval.service';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { Store } from '@ngrx/store';
import { selectCurrentUser } from '../../../store/selectors/auth.selectors';
import { logout } from '../../../store/actions/auth.actions';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';

@Component({
  selector: 'app-timesheet-approval',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatChipsModule,
    MatButtonToggleModule,
    ToolbarComponent,
  ],
  templateUrl: './timesheet-approval.component.html',
  styleUrls: ['./timesheet-approval.component.scss']
})
export class TimesheetApprovalComponent implements OnInit {
  // Sidenav links for MainLayoutComponent
  sidenavLinks = [
    { label: 'Dashboard', icon: 'dashboard', route: '/manager/dashboard' },
    { label: 'Projects', icon: 'work', route: '/manager/projects' },
    { label: 'Assignments', icon: 'assignment_ind', route: '/manager/assignments' },
    { label: 'Timesheet Approvals', icon: 'check_circle', route: '/manager/timesheet-approvals' },
    { label: 'Reports', icon: 'bar_chart', route: '/manager/reports' }
  ];
  private store = inject(Store);
  private router = inject(Router);
  private timesheetApprovalService = inject(TimesheetApprovalService);

  user$ = this.store.select(selectCurrentUser);

  private readonly baseColumns = ['employee', 'project', 'date', 'hours', 'status'];
  get displayedColumns() {
    return this.statusFilter === 'Pending' ? [...this.baseColumns, 'actions'] : this.baseColumns;
  }
  timesheets: any[] = [];
  statusFilter: 'Pending' | 'Approved' | 'Rejected' = 'Pending';
  statusOptions = ['Pending', 'Approved', 'Rejected'];

  ngOnInit(): void {
    this.loadTimesheetsByStatus(this.statusFilter);
  }

  loadTimesheetsByStatus(status: string): void {
    this.statusFilter = status as any;
    if (status === 'Pending') {
      this.timesheetApprovalService.getPendingTimesheets().subscribe({
        next: (data) => {
          this.timesheets = data.map(ts => ({
            ...ts,
            employee: ts.employeeName || ts.employee || ts.employeeId,
            project: ts.projectName || ts.project || ts.projectCodeId,
            date: ts.date,
            hours: ts.hoursWorked,
            status: ts.status
          }));
        },
        error: () => {
          this.timesheets = [];
        }
      });
    } else {
      this.timesheetApprovalService.getTimesheetsByStatus(status).subscribe({
        next: (data) => {
          this.timesheets = data.map(ts => ({
            ...ts,
            employee: ts.employeeName || ts.employee || ts.employeeId,
            project: ts.projectName || ts.project || ts.projectCodeId,
            date: ts.date,
            hours: ts.hoursWorked,
            status: ts.status
          }));
        },
        error: () => {
          this.timesheets = [];
        }
      });
    }
  }

  approveTimesheet(item: any): void {
    this.timesheetApprovalService.approveTimesheet(item.id).subscribe({
      next: () => {
        this.loadTimesheetsByStatus(this.statusFilter);
      },
      error: () => {
        alert('Failed to approve timesheet.');
      }
    });
  }

  rejectTimesheet(item: any): void {
    const comments = prompt('Enter rejection comments:');
    if (!comments) {
      alert('Rejection comments are required.');
      return;
    }
    this.timesheetApprovalService.rejectTimesheet(item.id, comments).subscribe({
      next: () => {
        this.loadTimesheetsByStatus(this.statusFilter);
      },
      error: () => {
        alert('Failed to reject timesheet.');
      }
    });
  }

  onLogout(): void {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}
