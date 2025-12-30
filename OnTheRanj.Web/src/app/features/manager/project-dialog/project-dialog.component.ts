import { Component, inject, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { ProjectCode } from '../../../core/models/project.model';

export interface ProjectDialogData {
  project?: ProjectCode;
  mode: 'create' | 'edit';
}

@Component({
  selector: 'app-project-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatSelectModule
  ],
  templateUrl: './project-dialog.component.html',
  styleUrls: ['./project-dialog.component.scss']
})
export class ProjectDialogComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ProjectDialogComponent>);
  
  projectForm: FormGroup;

  constructor(@Inject(MAT_DIALOG_DATA) public data: ProjectDialogData) {
    this.projectForm = this.fb.group({
      code: [data.project?.code || '', Validators.required],
      projectName: [data.project?.projectName || '', Validators.required],
      clientName: [data.project?.clientName || '', Validators.required],
      isBillable: [data.project?.isBillable || false],
      status: [data.project?.status || 'Active']
    });
  }

  onSubmit(): void {
    if (this.projectForm.valid) {
      this.dialogRef.close(this.projectForm.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
