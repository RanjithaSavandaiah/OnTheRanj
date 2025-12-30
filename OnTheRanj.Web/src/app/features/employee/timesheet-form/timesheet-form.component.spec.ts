import { provideMockStore } from '@ngrx/store/testing';
import { TimesheetFormComponent } from './timesheet-form.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, Input, Output, EventEmitter } from '@angular/core';

// Mocks for Angular Material and child components
@Component({selector: 'mat-card', template: '<ng-content></ng-content>'}) class MockMatCard {}
@Component({selector: 'mat-card-content', template: '<ng-content></ng-content>'}) class MockMatCardContent {}
@Component({selector: 'mat-form-field', template: '<ng-content></ng-content>'}) class MockMatFormField {}
@Component({selector: 'mat-label', template: '<ng-content></ng-content>'}) class MockMatLabel {}
@Component({selector: 'mat-select', template: '<ng-content></ng-content>'}) class MockMatSelect { @Input() formControlName: any; }
@Component({selector: 'mat-option', template: '<ng-content></ng-content>'}) class MockMatOption { @Input() value: any; }
@Component({selector: 'mat-error', template: '<ng-content></ng-content>'}) class MockMatError {}
@Component({selector: 'mat-datepicker-toggle', template: ''}) class MockMatDatepickerToggle { @Input() for: any; }
@Component({selector: 'mat-datepicker', template: ''}) class MockMatDatepicker {}
@Component({selector: 'mat-icon', template: '<ng-content></ng-content>'}) class MockMatIcon { @Input() matPrefix: any; }
@Component({selector: 'mat-input', template: ''}) class MockMatInput {}
@Component({selector: 'mat-button', template: '<ng-content></ng-content>'}) class MockMatButton { @Input() color: any; @Input() type: any; }
@Component({selector: 'mat-raised-button', template: '<ng-content></ng-content>'}) class MockMatRaisedButton { @Input() color: any; @Input() type: any; }
@Component({selector: 'mat-toolbar', template: '<ng-content></ng-content>'}) class MockMatToolbar {}
@Component({selector: 'mat-sidenav', template: '<ng-content></ng-content>'}) class MockMatSidenav {}
@Component({selector: 'mat-sidenav-container', template: '<ng-content></ng-content>'}) class MockMatSidenavContainer {}
@Component({selector: 'mat-sidenav-content', template: '<ng-content></ng-content>'}) class MockMatSidenavContent {}
@Component({selector: 'mat-list', template: '<ng-content></ng-content>'}) class MockMatList {}
@Component({selector: 'app-employee-layout', template: '<ng-content></ng-content>'}) class MockEmployeeLayout { @Input() sidenavLinks: any; }
@Component({selector: 'app-toolbar', template: '<ng-content></ng-content>'}) class MockToolbar { @Input() userFullName: any; @Output() logout = new EventEmitter(); }

describe('TimesheetFormComponent', () => {
  // Provide default selectors to avoid undefined errors
  const initialState = {};
  const mockSelectors = {
    selectCurrentUser: () => ({ id: 1, fullName: 'Test User' }),
    selectTimesheetError: () => null,
    selectTimesheetEntitiesSelector: () => ({})
  };

  function getProviders() {
    return [
      provideMockStore({ initialState, selectors: [
        { selector: 'selectCurrentUser', value: mockSelectors.selectCurrentUser() },
        { selector: 'selectTimesheetError', value: mockSelectors.selectTimesheetError() },
        { selector: 'selectTimesheetEntitiesSelector', value: mockSelectors.selectTimesheetEntitiesSelector() },
      ] }),
      { provide: Router, useValue: { navigate: () => {} } },
      { provide: ActivatedRoute, useValue: { snapshot: { params: {} } } }
    ];
  }

  it('should filter allowed assignments by date (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ get: () => ({ value: new Date() }), setValue: () => {} }) } as any,
      { select: () => ({ subscribe: () => {} }) } as any,
      {} as any,
      {} as any,
      {} as any
    );
    comp.allAssignments = [
      { id: 1, projectName: 'A', startDate: '2024-01-01', endDate: '2024-01-31', projectCodeId: 10 },
      { id: 2, projectName: 'B', startDate: '2024-02-01', endDate: null, projectCodeId: 20 }
    ];
    comp.timesheetForm = { get: () => ({ value: null, setValue: () => {} }) } as any;
    comp['filterAssignmentsByDate']('2024-01-15');
    expect(comp.allowedAssignments.length).toBe(1);
    expect(comp.allowedAssignments[0].projectName).toBe('A');
    comp['filterAssignmentsByDate']('2024-02-15');
    expect(comp.allowedAssignments.length).toBe(1);
    expect(comp.allowedAssignments[0].projectName).toBe('B');
  });

  it('should clear projectAssignmentId if not in allowedAssignments (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ get: () => ({ value: 1, setValue: function(val: any) { this.value = val; } }) }) } as any,
      { select: () => ({ subscribe: () => {} }) } as any,
      {} as any,
      {} as any,
      {} as any
    );
    comp.allAssignments = [
      { id: 1, projectName: 'A', startDate: '2024-01-01', endDate: '2024-01-31', projectCodeId: 10 }
    ];
    let projectAssignmentIdValue = 1;
    comp.timesheetForm = {
      get: (name: string) => name === 'projectAssignmentId' ? {
        value: projectAssignmentIdValue,
        setValue: (val: any) => { projectAssignmentIdValue = val; }
      } : { value: null, setValue: () => {} }
    } as any;
    comp['filterAssignmentsByDate']('2024-02-15');
    expect(projectAssignmentIdValue).toBe('');
  });
  
  it('should not submit if form is invalid (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ setErrors: () => {}, valid: false, value: {} }) } as any,
      { select: () => ({ subscribe: () => {} }) } as any,
      { navigate: () => {} } as any,
      {} as any,
      {} as any
    );
    comp.timesheetForm = { valid: false, value: {}, setErrors: () => {} } as any;
    expect(() => comp.onSubmit()).not.toThrow();
  });

  it('should alert if selected assignment is not found on submit (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ setValue: () => {}, valid: true, value: { date: new Date(), projectAssignmentId: 123, hoursWorked: 2, description: 'desc desc desc' } }) } as any,
      { select: () => ({ subscribe: () => {} }) } as any,
      { navigate: () => {} } as any,
      {} as any,
      {} as any
    );
    comp.allowedAssignments = [];
    comp.timesheetForm = { valid: true, value: { date: new Date(), projectAssignmentId: 123, hoursWorked: 2, description: 'desc desc desc' }, setValue: () => {} } as any;
    let alertCalled = false;
    const originalAlert = window.alert;
    window.alert = () => { alertCalled = true; };
    comp.onSubmit();
    expect(alertCalled).toBe(true);
    window.alert = originalAlert;
  });

  it('should dispatch updateTimesheet in edit mode (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ setValue: () => {}, valid: true, value: { date: new Date(), projectAssignmentId: 1, hoursWorked: 2, description: 'desc desc desc' } }) } as any,
      { dispatch: (action: any) => { comp._dispatched = action; }, select: () => ({ subscribe: () => {} }) } as any,
      { navigate: () => {} } as any,
      {} as any,
      {} as any
    );
    comp.allowedAssignments = [{ id: 1, projectName: 'A', startDate: '2024-01-01', endDate: null, projectCodeId: 10 }];
    comp.isEditMode = true;
    comp.timesheetId = 42;
    comp.currentUser = { id: 7 };
    comp.timesheetForm = { valid: true, value: { date: new Date(), projectAssignmentId: 1, hoursWorked: 2, description: 'desc desc desc' }, setValue: () => {} } as any;
    comp.store = { dispatch: (action: any) => { comp._dispatched = action; } };
    comp.onSubmit();
    expect(comp._dispatched.type).toContain('Update Timesheet');
  });

  it('should dispatch submitTimesheet in create mode (logic only)', () => {
    const comp: any = new TimesheetFormComponent(
      { group: () => ({ setValue: () => {}, valid: true, value: { date: new Date(), projectAssignmentId: 1, hoursWorked: 2, description: 'desc desc desc' } }) } as any,
      { dispatch: (action: any) => { comp._dispatched = action; }, select: () => ({ subscribe: () => {} }) } as any,
      { navigate: () => {} } as any,
      {} as any,
      {} as any
    );
    comp.allowedAssignments = [{ id: 1, projectName: 'A', startDate: '2024-01-01', endDate: null, projectCodeId: 10 }];
    comp.isEditMode = false;
    comp.currentUser = { id: 7 };
    comp.timesheetForm = { valid: true, value: { date: new Date(), projectAssignmentId: 1, hoursWorked: 2, description: 'desc desc desc' }, setValue: () => {} } as any;
    comp.store = { dispatch: (action: any) => { comp._dispatched = action; } };
    comp.onSubmit();
    expect(comp._dispatched.type).toContain('Submit Timesheet');
  });
});
