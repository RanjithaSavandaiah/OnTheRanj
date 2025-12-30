import { getISOWeek, getYear, startOfISOWeek, addWeeks, subWeeks, addYears, subYears, eachWeekOfInterval } from 'date-fns';
import { Component, OnInit } from '@angular/core';
import { employeeSidenavLinks } from '../../../shared/constants/employee-sidenav-links';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { startOfWeek, addDays, format } from 'date-fns';
import { TimesheetWeekService, TimesheetWeekEntry, TimesheetWeekSubmitRequest } from '../../../core/services/timesheet-week.service';
import { ProjectAssignmentService, ProjectAssignment } from '../../../core/services/project-assignment.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Store } from '@ngrx/store';
import { AppState } from '../../../store';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';
import { take } from 'rxjs/operators';
import { Observable } from 'rxjs';

interface WeekDay {
  date: Date;
  label: string;
}

import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { EmployeeLayoutComponent } from '../employee-layout.component';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';

@Component({
  selector: 'app-timesheet-week-view',
  templateUrl: './timesheet-week-view.component.html',
  styleUrls: ['./timesheet-week-view.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSnackBarModule,
    EmployeeLayoutComponent,
    ToolbarComponent,
  ]
})
export class TimesheetWeekViewComponent implements OnInit {
  incrementHour(entry: any, dayIndex: number) {
    const days = entry.get('days') as FormArray;
    const dayGroup = days.at(dayIndex);
    const hoursControl = dayGroup.get('hours');
    if (hoursControl) {
      const current = Number(hoursControl.value) || 0;
      if (current < 24) {
        hoursControl.setValue(current + 1);
      }
    }
  }

  decrementHour(entry: any, dayIndex: number) {
    const days = entry.get('days') as FormArray;
    const dayGroup = days.at(dayIndex);
    const hoursControl = dayGroup.get('hours');
    if (hoursControl) {
      const current = Number(hoursControl.value) || 0;
      if (current > 0) {
        hoursControl.setValue(current - 1);
      }
    }
  }
  currentWeekStart: Date = startOfWeek(new Date(), { weekStartsOn: 1 });
  weekOptions: { label: string, start: Date }[] = [];
  weekDays: WeekDay[] = [];
  displayedColumns: string[] = [];
  weekForm: FormGroup;
  projects: ProjectAssignment[] = [];
  currentUser$: Observable<any>;
  loading = false;
  employeeId: number | null = null;
  dayErrors: { [key: string]: string } = {};
  sidenavLinks = employeeSidenavLinks;
  tableRows: any[] = [];

  constructor(
    private fb: FormBuilder,
    private weekService: TimesheetWeekService,
    private projectAssignmentService: ProjectAssignmentService,
    private snackBar: MatSnackBar,
    private store: Store<AppState>
  ) {
    this.weekForm = this.fb.group({
      entries: this.fb.array([])
    });
    this.currentUser$ = this.store.select(AuthSelectors.selectCurrentUser);
  }

  // Expose addDays for template usage
  addDays(date: Date, days: number): Date {
    return addDays(date, days);
  }

  ngOnInit(): void {
    this.setWeek(this.currentWeekStart);
    this.generateWeekOptions(getYear(this.currentWeekStart));
    // Load employeeId and assigned projects from store
    this.store.select(AuthSelectors.selectCurrentUser).pipe(take(1)).subscribe(user => {
      this.employeeId = user?.id || null;
      if (this.employeeId) {
        this.projectAssignmentService.getActiveAssignments(this.employeeId).subscribe(assignments => {
          this.projects = assignments;
        });
      }
    });
  }

  setWeek(weekStart: Date) {
    this.currentWeekStart = startOfWeek(weekStart, { weekStartsOn: 1 });
    this.initWeekDays(this.currentWeekStart);
    this.displayedColumns = ['project', ...this.weekDays.map(d => d.label)];
    (this.weekForm.get('entries') as FormArray).clear();
    this.addProjectRow();
    this.tableRows = this.entries.controls.slice();
  }

  prevWeek() {
    this.setWeek(subWeeks(this.currentWeekStart, 1));
    this.generateWeekOptions(getYear(this.currentWeekStart));
  }
  nextWeek() {
    this.setWeek(addWeeks(this.currentWeekStart, 1));
    this.generateWeekOptions(getYear(this.currentWeekStart));
  }
  prevYear() {
    this.setWeek(subYears(this.currentWeekStart, 1));
    this.generateWeekOptions(getYear(this.currentWeekStart));
  }
  nextYear() {
    this.setWeek(addYears(this.currentWeekStart, 1));
    this.generateWeekOptions(getYear(this.currentWeekStart));
  }
  selectWeek(weekStart: Date) {
    this.setWeek(weekStart);
  }

  generateWeekOptions(year: number) {
    const jan1 = new Date(year, 0, 1);
    const dec31 = new Date(year, 11, 31);
    const weeks = eachWeekOfInterval({ start: jan1, end: dec31 }, { weekStartsOn: 1 });
    this.weekOptions = weeks.map((start, i) => ({
      label: `Week ${getISOWeek(start)} (${start.toLocaleDateString()})`,
      start
    }));
  }

  initWeekDays(start?: Date) {
    const base = start || startOfWeek(new Date(), { weekStartsOn: 1 });
    this.weekDays = Array.from({ length: 7 }, (_, i) => ({
      date: addDays(base, i),
      label: format(addDays(base, i), 'EEE dd')
    }));
  }

  get entries() {
    return this.weekForm.get('entries') as FormArray;
  }

  addProjectRow() {
    // Always create a row with all fields for all days, hours initialized as empty string
    // Ensure hours is always initialized to null for all days
    const daysArray = this.fb.array(
      this.weekDays.map(() => this.fb.group({
        hours: [null, [Validators.min(0), Validators.max(24)]],
        description: ['']
      }))
    );
    // Explicitly reset all hours fields to null (defensive, in case of future patching)
    daysArray.controls.forEach(dayGroup => {
      dayGroup.get('hours')?.setValue(null, { emitEvent: false });
    });
    this.entries.push(this.fb.group({
      projectCodeId: [null, Validators.required],
      days: daysArray
    }));
    // Force mat-table to update by reassigning the data source
    this.tableRows = this.entries.controls.slice();
  }

  removeProjectRow(index: number) {
    this.entries.removeAt(index);
    // Force mat-table to update by reassigning the data source
    this.tableRows = this.entries.controls.slice();
  }

  submitWeek() {
    if (this.weekForm.valid && this.employeeId) {
      this.loading = true;
      this.dayErrors = {};
      const formValue = this.weekForm.value;
      const entries: TimesheetWeekEntry[] = [];
      formValue.entries.forEach((entry: any) => {
        this.weekDays.forEach((day, dIdx) => {
          const hours = entry.days[dIdx].hours;
          const description = entry.days[dIdx].description;
          if (entry.projectCodeId && hours) {
            entries.push({
              projectCodeId: entry.projectCodeId,
              date: day.date.toISOString(),
              hoursWorked: hours,
              description: description || ''
            });
          }
        });
      });
      const payload: TimesheetWeekSubmitRequest = {
        employeeId: this.employeeId,
        entries
      };
      this.weekService.submitWeek(payload).subscribe({
        next: (res) => {
          this.loading = false;
          let hasError = false;
          if (res && res.results) {
            res.results.forEach((r: any) => {
              if (!r.success) {
                hasError = true;
                // Key: projectCodeId|date
                this.dayErrors[`${r.projectCodeId}|${r.date.substring(0, 10)}`] = r.message;
              }
            });
          }
          if (hasError) {
            this.snackBar.open('Some entries failed. See errors in the table.', 'Close', { duration: 5000 });
          } else {
            this.snackBar.open('Timesheet week submitted!', 'Close', { duration: 3000 });
          }
        },
        error: (err) => {
          this.loading = false;
          this.snackBar.open('Error submitting timesheet week', 'Close', { duration: 4000 });
        }
      });
    } else {
      this.weekForm.markAllAsTouched();
    }
  }

  getDayError(projectCodeId: number, date: Date): string | null {
    return this.dayErrors[`${projectCodeId}|${date.toISOString().substring(0, 10)}`] || null;
  }

  trackByIndex(index: number, item: any): number {
    return index;
  }
}
