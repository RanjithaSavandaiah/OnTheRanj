import { render, screen, fireEvent, waitFor } from '@testing-library/angular';
import { AssignmentDialogComponent, AssignmentDialogData } from './assignment-dialog.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('AssignmentDialogComponent', () => {
  const employees = [
    { id: 1, fullName: 'Alice', email: 'alice@example.com', role: 'Employee' as const, isActive: true },
    { id: 2, fullName: 'Bob', email: 'bob@example.com', role: 'Employee' as const, isActive: true }
  ];
  const projects = [
    { id: 1, code: 'P001', projectName: 'Project 1', clientName: 'ClientX', isBillable: true, status: 'Active' as const, createdAt: new Date(), createdBy: 1 },
    { id: 2, code: 'P002', projectName: 'Project 2', clientName: 'ClientY', isBillable: false, status: 'Active' as const, createdAt: new Date(), createdBy: 1 }
  ];
  const dialogRef = { close: (...args: any[]) => { (dialogRef.close as any).calls.push(args); } };
  (dialogRef.close as any).calls = [];
  (dialogRef.close as any).reset = () => { (dialogRef.close as any).calls = []; };

  function getDefaultData(isEdit = false, assignment?: any): AssignmentDialogData {
    return { employees, projects, isEdit, assignment };
  }

  it('should render form fields for new assignment', async () => {
    await render(AssignmentDialogComponent, {
      componentProperties: {},
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    expect(screen.getByLabelText(/Employee/i)).toBeTruthy();
    expect(screen.getByLabelText(/Project/i)).toBeTruthy();
    expect(screen.getByLabelText(/Start Date/i)).toBeTruthy();
    expect(screen.getByLabelText(/End Date/i)).toBeTruthy();
  });

  it('should disable fields in edit mode', async () => {
    const assignment = { employeeId: 1, projectCodeId: 1, startDate: '2025-12-01', endDate: '2025-12-31' };
    await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(true, assignment) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    // Directly query mat-select and input[formcontrolname] for disabled state
    const employeeSelect = document.querySelector('mat-select[formcontrolname="employeeId"]');
    const projectSelect = document.querySelector('mat-select[formcontrolname="projectCodeId"]');
    const startDateInput = document.querySelector('input[formcontrolname="startDate"]');
    const endDateInput = document.querySelector('input[formcontrolname="endDate"]');
    expect(employeeSelect?.getAttribute('aria-disabled')).toBe('true');
    expect(projectSelect?.getAttribute('aria-disabled')).toBe('true');
    expect((startDateInput as HTMLInputElement).disabled).toBe(true);
    expect((endDateInput as HTMLInputElement).disabled).toBe(false);
  });

  it('should call dialogRef.close with payload on save', async () => {
    (dialogRef.close as any).reset();
    const { fixture } = await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    // Open and select employee (mat-select uses overlay)
    fireEvent.mouseDown(screen.getByLabelText(/Employee/i));
    const overlayContainer = document.body.querySelector('.cdk-overlay-container');
    const aliceOption = Array.from(overlayContainer?.querySelectorAll('mat-option') || []).find(opt => opt.textContent?.includes('Alice'));
    if (aliceOption) fireEvent.click(aliceOption);
    // Open and select project
    fireEvent.mouseDown(screen.getByLabelText(/Project/i));
    const project2Option = Array.from(overlayContainer?.querySelectorAll('mat-option') || []).find(opt => opt.textContent?.includes('Project 2'));
    if (project2Option) fireEvent.click(project2Option);
    // Set form values directly via component instance
    fixture.componentInstance.form.patchValue({
      employeeId: 1,
      projectCodeId: 2,
      startDate: '2025-12-01',
      endDate: '2025-12-31'
    });
    fixture.detectChanges();
    // Set start and end date (native input)
    const startDateInput = screen.getByLabelText(/Start Date/i) as HTMLInputElement;
    const endDateInput = screen.getByLabelText(/End Date/i) as HTMLInputElement;
    // Simulate create button click
    const createBtn = await screen.findByRole('button', { name: /create/i });
    fireEvent.click(createBtn);
    await waitFor(() => {
      const calls = (dialogRef.close as any).calls;
      expect(calls.length).toBe(1);
      expect(calls[0][0]).toEqual({
        employeeId: 1,
        projectCodeId: 2,
        startDate: '2025-12-01',
        endDate: '2025-12-31'
      });
    });
  });

  it('should close dialog without payload on cancel', async () => {
    (dialogRef.close as any).reset();
    await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    const cancelBtn = screen.getByRole('button', { name: /cancel/i });
    fireEvent.click(cancelBtn);
    const cancelCalls = (dialogRef.close as any).calls;
    expect(cancelCalls.length).toBe(1);
    expect(cancelCalls[0][0]).toBeUndefined();
  });

  it('should not call dialogRef.close if form is invalid (missing required fields)', async () => {
    (dialogRef.close as any).reset();
    await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    // Do not fill any fields
    const createBtn = await screen.findByRole('button', { name: /create/i });
    fireEvent.click(createBtn);
    await waitFor(() => {
      expect((dialogRef.close as any).calls.length).toBe(0);
    });
  });

  it('should not call dialogRef.close if end date is before start date', async () => {
    (dialogRef.close as any).reset();
    const { fixture } = await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    fixture.componentInstance.form.patchValue({
      employeeId: 1,
      projectCodeId: 2,
      startDate: '2025-12-31',
      endDate: '2025-12-01'
    });
    fixture.detectChanges();
    const createBtn = await screen.findByRole('button', { name: /create/i });
    fireEvent.click(createBtn);
    await waitFor(() => {
      expect((dialogRef.close as any).calls.length).toBe(0);
    });
  });

  it('should call dialogRef.close with payload in edit mode', async () => {
    (dialogRef.close as any).reset();
    const assignment = { employeeId: 2, projectCodeId: 1, startDate: '2025-12-01', endDate: '2025-12-31' };
    const { fixture } = await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(true, assignment) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    // Only endDate should be editable
    // Patch endDate using setValue on the enabled control
    fixture.componentInstance.form.get('endDate')?.setValue('2026-01-01');
    fixture.detectChanges();
    // Button label is 'Update' in edit mode
    const updateBtn = await screen.findByRole('button', { name: /update/i });
    fireEvent.click(updateBtn);
    await waitFor(() => {
      const calls = (dialogRef.close as any).calls;
      expect(calls.length).toBe(1);
      expect(calls[0][0]).toEqual({
        employeeId: 2,
        projectCodeId: 1,
        startDate: '2025-12-01',
        endDate: '2026-01-01'
      });
    });
  });

  it('should not call dialogRef.close if dialog is already closed', async () => {
    (dialogRef.close as any).reset();
    let closed = false;
    const customDialogRef = {
      close: (...args: any[]) => {
        if (!closed) {
          (customDialogRef.close as any).calls.push(args);
          closed = true;
        }
      }
    };
    (customDialogRef.close as any).calls = [];
    await render(AssignmentDialogComponent, {
      providers: [
        { provide: MatDialogRef, useValue: customDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: getDefaultData(false) },
        FormBuilder
      ],
      imports: [ReactiveFormsModule, NoopAnimationsModule]
    });
    // Simulate double click on cancel
    const cancelBtn = screen.getByRole('button', { name: /cancel/i });
    fireEvent.click(cancelBtn);
    fireEvent.click(cancelBtn);
    const calls = (customDialogRef.close as any).calls;
    expect(calls.length).toBe(1);
  });
});
