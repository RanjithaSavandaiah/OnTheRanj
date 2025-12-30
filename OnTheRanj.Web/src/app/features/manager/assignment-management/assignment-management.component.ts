import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { ProjectAssignmentService, ProjectAssignment } from '../../../core/services/project-assignment.service';
import { AssignmentDialogComponent } from './assignment-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { ProjectCodeService } from '../../../core/services/project-code.service';
import { User } from '../../../core/models/user.model';
import { ProjectCode } from '../../../core/models/project.model';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { MatDialogModule } from '@angular/material/dialog';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';

@Component({
  selector: 'app-assignment-management',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatListModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    MatDialogModule,
    ToolbarComponent,
  ],
  styleUrls: ['./assignment-management.component.scss'],
  templateUrl: './assignment-management.component.html'
})

export class AssignmentManagementComponent implements OnInit {
    // Sidenav links for MainLayoutComponent
    sidenavLinks = [
      { label: 'Dashboard', icon: 'dashboard', route: '/manager/dashboard' },
      { label: 'Projects', icon: 'work', route: '/manager/projects' },
      { label: 'Assignments', icon: 'assignment_ind', route: '/manager/assignments' },
      { label: 'Timesheet Approvals', icon: 'check_circle', route: '/manager/timesheet-approvals' },
      { label: 'Reports', icon: 'bar_chart', route: '/manager/reports' }
    ];
  assignments: ProjectAssignment[] = [];
  displayedColumns = ['id', 'employee', 'projectCode', 'startDate', 'endDate', 'actions'];
  
  // Helper to get the label for the selected employee (including All Employees)
  get selectedEmployeeLabel(): string {
    if (this.selectedEmployeeId == null) return 'All Employees';
    const emp = this.employees.find((e: User) => e.id === this.selectedEmployeeId);
    return emp ? emp.fullName : 'Select Employee';
  }
  loading = false;
  employees: User[] = [];
  projects: ProjectCode[] = [];
  error: string | null = null;
  selectedEmployeeId: number | null = null; // null means 'All Employees' by default
  auth = inject(AuthService);
  router = inject(Router);

  // Custom compare function for mat-select to handle null (All Employees) correctly
  compareEmployee(a: number | null, b: number | null): boolean {
    return a === b;
  }

  constructor(
    private projectAssignmentService: ProjectAssignmentService,
    private userService: UserService,
    private projectCodeService: ProjectCodeService,
    private dialog: MatDialog
  ) {}

  get currentUser() {
    return this.auth.currentUser();
  }

  ngOnInit(): void {
    this.userService.getAll().subscribe({
      next: users => {
        // Only include active employees (not managers or inactive)
        this.employees = users.filter(u => u.role === 'Employee' && u.isActive);
        this.selectedEmployeeId = null;
        this.fetchAssignments();
      },
      error: err => this.handleError(err)
    });
    this.projectCodeService.getActive().subscribe({
      next: projects => this.projects = projects,
      error: err => this.handleError(err)
    });
  }

  onEmployeeChange() {
    this.fetchAssignments();
  }

  fetchAssignments() {
    this.loading = true;
    if (this.selectedEmployeeId == null) {
      // Fetch all assignments
      this.projectAssignmentService.getAll().subscribe({
        next: (data) => {
          this.assignments = (data || []).sort((a, b) => a.id - b.id); // Sort by id ascending
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          this.handleError(err);
        }
      });
    } else {
      // Fetch assignments for selected employee
      this.projectAssignmentService.getAllByEmployee(this.selectedEmployeeId).subscribe({
        next: (data) => {
          this.assignments = (data || []).sort((a, b) => a.id - b.id); // Sort by id ascending
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          this.handleError(err);
        }
      });
    }
  }

  openCreateDialog() {
    // Only pass active employees to the dialog
    const dialogRef = this.dialog.open(AssignmentDialogComponent, {
      data: {
        employees: this.employees,
        projects: this.projects,
        isEdit: false
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectAssignmentService.create(result).subscribe(() => this.fetchAssignments());
      }
    });
  }

  openEditDialog(assignment: ProjectAssignment) {
    const dialogRef = this.dialog.open(AssignmentDialogComponent, {
      data: {
        employees: this.employees,
        projects: this.projects,
        assignment,
        isEdit: true
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectAssignmentService.update(assignment.id, result).subscribe(() => this.fetchAssignments());
      }
    });
  }

  deleteAssignment(id: number) {
    if (confirm('Are you sure you want to delete this assignment?')) {
      this.projectAssignmentService.delete(id).subscribe(() => this.fetchAssignments());
    }
  }

  onLogout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  handleError(err: any) {
    if (err.status === 401) {
      this.error = 'You are not authorized. Please log in again.';
      setTimeout(() => this.onLogout(), 1500);
    } else {
      this.error = 'An error occurred while loading data.';
    }
  }
}
