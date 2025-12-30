import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Store } from '@ngrx/store';
import { selectCurrentUser } from '../../../store/selectors/auth.selectors';
import { logout } from '../../../store/actions/auth.actions';
import { ProjectService } from '../../../core/services/project.service';
import { UserService } from '../../../core/services/user.service';
import { TimesheetService } from '../../../core/services/timesheet.service';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    ToolbarComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);
  private projectService = inject(ProjectService);
  private userService = inject(UserService);
  private timesheetService = inject(TimesheetService);

  user$ = this.store.select(selectCurrentUser);

  projectCount = 0;
  employeeCount = 0;
  timesheetCount = 0;
  pendingApprovalCount = 0;

  ngOnInit(): void {
    this.projectService.getAllProjects().subscribe(projects => {
      this.projectCount = projects.length;
    });
    this.userService.getAll().subscribe(users => {
      this.employeeCount = users.filter(u => u.role === 'Employee').length;
    });
    this.timesheetService.getAllTimesheets().subscribe((all: any[]) => {
      this.timesheetCount = all.length;
    });
    this.timesheetService.getPendingTimesheets().subscribe((pending: any[]) => {
      this.pendingApprovalCount = pending.length;
    });
  }

  onLogout(): void {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}

