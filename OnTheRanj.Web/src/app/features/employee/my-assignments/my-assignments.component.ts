import { employeeSidenavLinks } from '../../../shared/constants/employee-sidenav-links';
import { Component, OnInit } from '@angular/core';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';
import { EmployeeLayoutComponent } from '../employee-layout.component';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { AsyncPipe, DatePipe, NgIf, NgFor, NgClass } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppState } from '../../../store';
import { ProjectAssignmentService, ProjectAssignment } from '../../../core/services/project-assignment.service';
import { Observable } from 'rxjs';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';

@Component({
  selector: 'app-my-assignments',
  standalone: true,
  imports: [
    EmployeeLayoutComponent,
    ToolbarComponent,
    MatCardModule,
    MatTableModule,
    MatIconModule,
    AsyncPipe,
    DatePipe,
    NgIf,
  ],
  templateUrl: './my-assignments.component.html',
  styleUrls: []
})
export class MyAssignmentsComponent implements OnInit {
  sidenavLinks = employeeSidenavLinks;
  assignments: ProjectAssignment[] = [];
  displayedColumns: string[] = ['projectName', 'startDate', 'endDate'];
  currentUser$!: Observable<any>;

  constructor(
    private store: Store<AppState>,
    private projectAssignmentService: ProjectAssignmentService
  ) {}

  ngOnInit(): void {
    this.currentUser$ = this.store.select(AuthSelectors.selectCurrentUser);
    this.currentUser$.subscribe(user => {
      if (user && user.id) {
        this.projectAssignmentService.getActiveAssignments(user.id).subscribe(assignments => {
          this.assignments = assignments;
        });
      }
    });
  }

  onLogout() {
    // Dispatch logout action or navigate to logout route
    // this.store.dispatch(AuthActions.logout());
    window.location.href = '/logout';
  }
}
