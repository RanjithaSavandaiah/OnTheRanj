import { employeeSidenavLinks } from '../../../shared/constants/employee-sidenav-links';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { ProjectAssignmentService, ProjectAssignment } from '../../../core/services/project-assignment.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { EmployeeLayoutComponent } from '../employee-layout.component';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';
import { AppState } from '../../../store';
import { MatDialogRef } from '@angular/material/dialog';
import { Optional } from '@angular/core';
import * as TimesheetActions from '../../../store/actions/timesheet.actions';
import * as TimesheetSelectors from '../../../store/selectors/timesheet.selectors';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';

/**
 * Timesheet form component for creating/editing timesheets
 */
@Component({
  selector: 'app-timesheet-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    EmployeeLayoutComponent,
    ToolbarComponent,
  ],
  templateUrl: './timesheet-form.component.html',
  styleUrls: ['./timesheet-form.component.scss']
})
export class TimesheetFormComponent implements OnInit {
  sidenavLinks = employeeSidenavLinks;
  timesheetForm!: FormGroup;
  allAssignments: any[] = [];
  allowedAssignments: any[] = [];
  currentUser$!: Observable<any>;
  currentUser: any = null;
  isEditMode = false;
  timesheetId?: number;
  minDate: Date | null = null;
  maxDate: Date | null = null;
  selectedAssignment: any = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private store: Store<AppState>,
    private router: Router,
    private route: ActivatedRoute,
    private projectAssignmentService: ProjectAssignmentService,
    @Optional() private dialogRef?: MatDialogRef<TimesheetFormComponent>
  ) { }

  ngOnInit(): void {
    this.currentUser$ = this.store.select(AuthSelectors.selectCurrentUser);
    this.initializeForm();
    this.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user && user.id) {
        this.projectAssignmentService.getActiveAssignments(user.id).subscribe(assignments => {
          this.allAssignments = assignments;
          this.filterAssignmentsByDate(this.timesheetForm.get('date')?.value);
          this.checkEditMode(); // Ensure assignments are loaded before patching form
        });
      }
    });
    // Re-filter assignments when the date changes
    this.timesheetForm?.get('date')?.valueChanges.subscribe(date => {
      this.filterAssignmentsByDate(date);
    });
    // Subscribe to error state
    this.store.select(TimesheetSelectors.selectTimesheetError).subscribe(error => {
      this.errorMessage = error;
    });
  }

  private filterAssignmentsByDate(date: Date | string) {
    if (!date) {
      this.allowedAssignments = [];
      return;
    }
    const selectedDate = date instanceof Date ? date : new Date(date);
    this.allowedAssignments = this.allAssignments.filter(a => {
      const start = new Date(a.startDate);
      const end = a.endDate ? new Date(a.endDate) : null;
      return start <= selectedDate && (!end || end >= selectedDate);
    });
    // If the current selection is not valid, clear it
    const selectedId = this.timesheetForm.get('projectAssignmentId')?.value;
    if (selectedId && !this.allowedAssignments.some(a => a.id === selectedId)) {
      this.timesheetForm.get('projectAssignmentId')?.setValue('');
    }
  }

  /**
   * Initialize form with validators
   */
  private initializeForm(): void {
    this.timesheetForm = this.fb.group({
      date: [new Date(), Validators.required],
      projectAssignmentId: ['', Validators.required],
      hoursWorked: ['', [Validators.required, Validators.min(0.5), Validators.max(24)]],
      description: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  /**
   * Check if we're in edit mode
   */
  private checkEditMode(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEditMode = true;
      this.timesheetId = +id;
      // Load timesheet data and patch the form
      this.store.select(TimesheetSelectors.selectTimesheetEntitiesSelector).subscribe(entities => {
        const ts = entities[this.timesheetId!];
        if (ts) {
          this.timesheetForm.patchValue({
            date: new Date(ts.date),
            projectAssignmentId: this.allAssignments.find(a => a.projectCodeId === ts.projectCodeId)?.id || '',
            hoursWorked: ts.hoursWorked,
            description: ts.description
          });
        }
      });
    }
  }

  /**
   * Handle form submission
   */
  onSubmit(): void {
    if (this.timesheetForm.valid) {
      const formValue = this.timesheetForm.value;
      // Find the selected assignment
      const selectedAssignment = this.allowedAssignments.find(a => a.id === formValue.projectAssignmentId);
      if (!selectedAssignment) {
        alert('Invalid project selection.');
        return;
      }
      // Map form value to TimesheetRequest with correct projectCodeId
      const request = {
        employeeId: this.currentUser?.id,
        projectCodeId: selectedAssignment.projectCodeId,
        date: formValue.date instanceof Date ? formValue.date.toISOString() : formValue.date,
        hoursWorked: formValue.hoursWorked,
        description: formValue.description
      };
      if (this.isEditMode && this.timesheetId) {
        this.store.dispatch(
          TimesheetActions.updateTimesheet({
            id: this.timesheetId,
            request
          })
        );
      } else {
        this.store.dispatch(
          TimesheetActions.submitTimesheet({ request })
        );
      }
      if (this.dialogRef) {
        this.dialogRef.close();
      } else {
        this.router.navigate(['/employee/timesheets']);
      }
    }
  }

  /** Cancel and go back */
  onCancel(): void {
    if (this.dialogRef) {
      this.dialogRef.close();
    } else {
      this.router.navigate(['/employee/timesheets']);
    }
  }
  // Getter methods for template access
  get date() { return this.timesheetForm.get('date'); }
  get projectAssignmentId() { return this.timesheetForm.get('projectAssignmentId'); }
  get hoursWorked() { return this.timesheetForm.get('hoursWorked'); }
  get description() { return this.timesheetForm.get('description'); }
}
