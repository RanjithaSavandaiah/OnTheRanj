import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProjectDialogComponent, ProjectDialogData } from './project-dialog.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';


describe('ProjectDialogComponent (create mode)', () => {
  let component: ProjectDialogComponent;
  let fixture: ComponentFixture<ProjectDialogComponent>;
  let closeCalls: any[];
  let dialogRefSpy: { close: (...args: any[]) => void };
  const mockData: ProjectDialogData = {
    mode: 'create',
    project: {
      id: 1,
      code: 'P001',
      projectName: 'Test Project',
      clientName: 'Test Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    }
  };
  beforeEach(async () => {
    closeCalls = [];
    dialogRefSpy = { close: (...args: any[]) => { closeCalls.push(args); } };
    await TestBed.configureTestingModule({
      imports: [
        ProjectDialogComponent,
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockData }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(ProjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should initialize form with project data', () => {
    expect(component.projectForm.value).toEqual({
      code: 'P001',
      projectName: 'Test Project',
      clientName: 'Test Client',
      isBillable: true,
      status: 'Active'
    });
  });
  it('should close dialog with form value on submit if valid', () => {
    component.projectForm.setValue({
      code: 'P002',
      projectName: 'New Project',
      clientName: 'New Client',
      isBillable: false,
      status: 'Inactive'
    });
    component.onSubmit();
    expect(closeCalls.length).toBe(1);
    expect(closeCalls[0][0]).toEqual({
      code: 'P002',
      projectName: 'New Project',
      clientName: 'New Client',
      isBillable: false,
      status: 'Inactive'
    });
  });
  it('should not close dialog on submit if form is invalid', () => {
    component.projectForm.patchValue({ code: '' });
    component.onSubmit();
    expect(closeCalls.length).toBe(0);
  });
  it('should close dialog on cancel', () => {
    component.onCancel();
    expect(closeCalls.length).toBe(1);
    expect(closeCalls[0].length).toBe(0);
  });
  it('should render form fields', () => {
    const codeInput = fixture.debugElement.query(By.css('input[formcontrolname="code"]'));
    const projectNameInput = fixture.debugElement.query(By.css('input[formcontrolname="projectName"]'));
    const clientNameInput = fixture.debugElement.query(By.css('input[formcontrolname="clientName"]'));
    expect(codeInput).toBeTruthy();
    expect(projectNameInput).toBeTruthy();
    expect(clientNameInput).toBeTruthy();
  });
  it('should not show status select and code not readonly in create mode', () => {
    const statusSelect = fixture.debugElement.query(By.css('mat-select[formcontrolname="status"]'));
    expect(statusSelect).toBeNull();
    const codeInput = fixture.debugElement.query(By.css('input[formcontrolname="code"]'));
    expect(codeInput.attributes['readonly']).toBeUndefined();
  });
  it('should toggle isBillable checkbox', () => {
    const billableCheckbox = fixture.debugElement.query(By.css('mat-checkbox[formcontrolname="isBillable"]'));
    expect(component.projectForm.get('isBillable')?.value).toBe(true);
    component.projectForm.get('isBillable')?.setValue(false);
    fixture.detectChanges();
    expect(component.projectForm.get('isBillable')?.value).toBe(false);
  });
  it('should show error messages for required fields', () => {
    component.projectForm.get('code')?.setValue('');
    component.projectForm.get('projectName')?.setValue('');
    component.projectForm.get('clientName')?.setValue('');
    component.projectForm.get('code')?.markAsTouched();
    component.projectForm.get('projectName')?.markAsTouched();
    component.projectForm.get('clientName')?.markAsTouched();
    fixture.detectChanges();
    const codeError = fixture.debugElement.query(By.css('mat-error'));
    expect(codeError.nativeElement.textContent).toContain('Project code is required');
  });
});

describe('ProjectDialogComponent (edit mode)', () => {
  let component: ProjectDialogComponent;
  let fixture: ComponentFixture<ProjectDialogComponent>;
  let closeCalls: any[];
  let dialogRefSpy: { close: (...args: any[]) => void };
  const mockData: ProjectDialogData = {
    mode: 'edit',
    project: {
      id: 1,
      code: 'P001',
      projectName: 'Test Project',
      clientName: 'Test Client',
      isBillable: true,
      status: 'Active',
      createdAt: new Date(),
      createdBy: 1
    }
  };
  beforeEach(async () => {
    closeCalls = [];
    dialogRefSpy = { close: (...args: any[]) => { closeCalls.push(args); } };
    await TestBed.configureTestingModule({
      imports: [
        ProjectDialogComponent,
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockData }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(ProjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should show status select and code readonly in edit mode', () => {
    const statusSelect = fixture.debugElement.query(By.css('mat-select[formcontrolname="status"]'));
    expect(statusSelect).toBeTruthy();
    const codeInput = fixture.debugElement.query(By.css('input[formcontrolname="code"]'));
    expect(codeInput.attributes['readonly']).toBeDefined();
  });
});
