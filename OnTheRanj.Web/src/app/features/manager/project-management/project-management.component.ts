import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Store } from '@ngrx/store';
import { Actions, ofType } from '@ngrx/effects';
import { selectCurrentUser } from '../../../store/selectors/auth.selectors';
import { selectProjects } from '../../../store/selectors/project.selectors';
import { logout } from '../../../store/actions/auth.actions';
import { loadProjects, createProject, updateProject, deleteProject, createProjectSuccess, createProjectFailure, updateProjectSuccess, updateProjectFailure, deleteProjectSuccess, deleteProjectFailure } from '../../../store/actions/project.actions';
import { ProjectCode } from '../../../core/models/project.model';
import { ProjectDialogComponent } from '../project-dialog/project-dialog.component';

@Component({
  selector: 'app-project-management',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatTableModule,
    MatChipsModule,
    MatTooltipModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './project-management.component.html',
  styleUrls: ['./project-management.component.scss']
})
export class ProjectManagementComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private actions$ = inject(Actions);
  
  user$ = this.store.select(selectCurrentUser);
  projects$ = this.store.select(selectProjects);
  
  displayedColumns = ['code', 'projectName', 'clientName', 'isBillable', 'status', 'actions'];

  ngOnInit(): void {
    this.store.dispatch(loadProjects());
    
    // Listen for success actions
    this.actions$.pipe(ofType(createProjectSuccess)).subscribe(() => {
      this.snackBar.open('Project created successfully', 'Close', { duration: 3000 });
    });
    
    this.actions$.pipe(ofType(updateProjectSuccess)).subscribe(() => {
      this.snackBar.open('Project updated successfully', 'Close', { duration: 3000 });
      this.store.dispatch(loadProjects());
    });
    
    this.actions$.pipe(ofType(deleteProjectSuccess)).subscribe(() => {
      this.snackBar.open('Project deleted successfully', 'Close', { duration: 3000 });
    });
    
    // Listen for failure actions
    this.actions$.pipe(ofType(createProjectFailure, updateProjectFailure, deleteProjectFailure)).subscribe((action: any) => {
      this.snackBar.open(`Error: ${action.error}`, 'Close', { duration: 5000 });
    });
  }

  onAddProject(): void {
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '500px',
      data: { mode: 'create' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (
        result &&
        result.code &&
        result.projectName &&
        result.clientName &&
        typeof result.isBillable === 'boolean'
      ) {
        this.store.dispatch(createProject({
          request: {
            code: result.code,
            projectName: result.projectName,
            clientName: result.clientName,
            isBillable: result.isBillable
          }
        }));
      }
    });
  }

  onEditProject(project: ProjectCode): void {
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '500px',
      data: { project, mode: 'edit' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (
        result &&
        result.code &&
        result.projectName &&
        result.clientName &&
        typeof result.isBillable === 'boolean' &&
        result.status
      ) {
        this.store.dispatch(updateProject({
          id: project.id,
          request: {
            id: project.id,
            code: result.code,
            projectName: result.projectName,
            clientName: result.clientName,
            isBillable: result.isBillable,
            status: result.status
          }
        }));
      }
    });
  }

  onDeleteProject(project: ProjectCode): void {
    if (!project) return;
    if (confirm(`Are you sure you want to delete project "${project.projectName}"?`)) {
      this.store.dispatch(deleteProject({ id: project.id }));
    }
  }

  onLogout(): void {
    this.store.dispatch(logout());
    this.router.navigate(['/login']);
  }
}
