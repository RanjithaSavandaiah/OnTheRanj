import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

/**
 * Application routes with lazy loading and guards
 */
export const routes: Routes = [
  // Public routes
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },

  // Employee routes - protected by auth guard
  {
    path: 'employee',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Employee' },
    children: [
      {
        path: 'timesheets',
        loadComponent: () =>
          import('./features/employee/timesheet-list/timesheet-list.component').then(m => m.TimesheetListComponent)
      },
      {
        path: 'timesheets/new',
        loadComponent: () =>
          import('./features/employee/timesheet-form/timesheet-form.component').then(m => m.TimesheetFormComponent)
      },
      {
        path: 'timesheets/:id/edit',
        loadComponent: () =>
          import('./features/employee/timesheet-form/timesheet-form.component').then(m => m.TimesheetFormComponent)
      },
      {
        path: 'timesheets/week',
        loadComponent: () =>
          import('./features/employee/timesheet-week-view/timesheet-week-view.component').then(m => m.TimesheetWeekViewComponent)
      },
      {
        path: 'assignments',
        loadComponent: () =>
          import('./features/employee/my-assignments/my-assignments.component').then(m => m.MyAssignmentsComponent)
      },
      { path: '', redirectTo: 'timesheets', pathMatch: 'full' }
    ]
  },

  // Manager routes - protected by auth and role guards
  {
    path: 'manager',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Manager' },
    loadComponent: () => import('./features/manager/manager-layout.component').then(m => m.ManagerLayoutComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/manager/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'projects',
        loadComponent: () =>
          import('./features/manager/project-management/project-management.component').then(m => m.ProjectManagementComponent)
      },
      {
        path: 'assignments',
        loadComponent: () =>
          import('./features/manager/assignment-management/assignment-management.component').then(m => m.AssignmentManagementComponent)
      },
      {
        path: 'approvals',
        loadComponent: () =>
          import('./features/manager/timesheet-approval/timesheet-approval.component').then(m => m.TimesheetApprovalComponent)
      },
      {
        path: 'reports',
        loadComponent: () =>
          import('./features/manager/reports/reports.component').then(m => m.ReportsComponent)
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // Default and error routes
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'unauthorized',
    loadComponent: () =>
      import('./shared/components/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  { path: '**', redirectTo: '/login' }
];
