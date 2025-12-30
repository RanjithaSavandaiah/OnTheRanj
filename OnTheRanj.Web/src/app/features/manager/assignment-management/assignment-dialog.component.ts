import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { User } from '../../../core/models/user.model';
import { ProjectCode } from '../../../core/models/project.model';
import { provideNativeDateAdapter } from '@angular/material/core';

export interface AssignmentDialogData {
  employees: User[];
  projects: ProjectCode[];
  assignment?: any;
  isEdit: boolean;
}

@Component({
  selector: 'app-assignment-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, MatInputModule, MatDatepickerModule, MatSelectModule],
  templateUrl: './assignment-dialog.component.html',
  styleUrls: ['./assignment-dialog.component.scss'],
  providers: [provideNativeDateAdapter()]
})
export class AssignmentDialogComponent implements OnInit {
  form: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AssignmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AssignmentDialogData,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      employeeId: [data.assignment?.employeeId || '', Validators.required],
      projectCodeId: [data.assignment?.projectCodeId || '', Validators.required],
      startDate: [data.assignment?.startDate || '', Validators.required],
      endDate: [data.assignment?.endDate || '']
    });
    if (data.isEdit) {
      this.form.get('employeeId')?.disable();
      this.form.get('projectCodeId')?.disable();
      this.form.get('startDate')?.disable();
    }
  }

  ngOnInit(): void {}

  save() {
    if (this.form.valid) {
      const raw = this.form.getRawValue();
      // Validate endDate >= startDate if both are present
      if (raw.startDate && raw.endDate && new Date(raw.endDate) < new Date(raw.startDate)) {
        return;
      }
      const payload: any = {
        employeeId: Number(raw.employeeId),
        projectCodeId: Number(raw.projectCodeId),
        startDate: raw.startDate ? this.formatDateOnly(raw.startDate) : null
      };
      if (raw.endDate) {
        payload.endDate = this.formatDateOnly(raw.endDate);
      }
      this.dialogRef.close(payload);
    }
  }

  // Helper to format a Date or string as YYYY-MM-DD
  private formatDateOnly(date: any): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  cancel() {
    this.dialogRef.close();
  }
}
