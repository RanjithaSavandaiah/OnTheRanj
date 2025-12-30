import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { Observable, of, throwError, Subject } from 'rxjs';
import { TimesheetEffects } from './timesheet.effects';
import { TimesheetService } from '../../core/services/timesheet.service';
import * as TimesheetActions from '../actions/timesheet.actions';
import * as TimesheetSubmitActions from '../actions/timesheet.submit.actions';
import * as AuthSelectors from '../selectors/auth.selectors';
import { Action } from '@ngrx/store';

describe('TimesheetEffects', () => {
  let actions$: Subject<Action>;
  let effects: TimesheetEffects;
  let timesheetService: any;
  let store: MockStore;

  beforeEach(() => {
    timesheetService = {
      getEmployeeTimesheets: (...args: any[]) => {},
      submitDraftTimesheet: (...args: any[]) => {},
      getPendingTimesheets: (...args: any[]) => {},
      submitTimesheet: (...args: any[]) => {},
      updateTimesheet: (...args: any[]) => {},
      deleteTimesheet: (...args: any[]) => {},
      reviewTimesheet: (...args: any[]) => {},
    };
    actions$ = new Subject<Action>();
    TestBed.configureTestingModule({
      providers: [
        TimesheetEffects,
        provideMockActions(() => actions$),
        provideMockStore({
          selectors: [
            { selector: AuthSelectors.selectCurrentUser, value: { id: 1 } }
          ]
        }),
        { provide: TimesheetService, useValue: timesheetService }
      ]
    });
    effects = TestBed.inject(TimesheetEffects);
    store = TestBed.inject(MockStore);
  });

  it('should dispatch loadEmployeeTimesheetsSuccess on loadEmployeeTimesheets$', () => {
    const timesheets = [{
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    }];
    timesheetService.getEmployeeTimesheets = () => of(timesheets);
    let emitted;
    effects.loadEmployeeTimesheets$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.loadEmployeeTimesheets({ employeeId: 1, startDate: new Date(), endDate: new Date() }));
    expect(emitted).toEqual(TimesheetActions.loadEmployeeTimesheetsSuccess({ timesheets }));
  });

  it('should dispatch loadEmployeeTimesheetsFailure on error', () => {
    timesheetService.getEmployeeTimesheets = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.loadEmployeeTimesheets$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.loadEmployeeTimesheets({ employeeId: 1, startDate: new Date(), endDate: new Date() }));
    expect(emitted).toEqual(TimesheetActions.loadEmployeeTimesheetsFailure({ error: 'fail' }));
  });

  it('should dispatch submitDraftTimesheetSuccess and loadEmployeeTimesheets on submitDraftTimesheet$', () => {
    const timesheet = {
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    };
    timesheetService.submitDraftTimesheet = () => of(timesheet);
    let results: any[] = [];
    effects.submitDraftTimesheet$.subscribe(result => results.push(result));
    actions$.next(TimesheetSubmitActions.submitDraftTimesheet({ id: 1 }));
    expect(results[0]).toEqual(TimesheetSubmitActions.submitDraftTimesheetSuccess({ timesheet }));
    expect(results[1]).toEqual(TimesheetActions.loadEmployeeTimesheets({ employeeId: 1 }));
  });

  it('should dispatch submitDraftTimesheetFailure on error', () => {
    timesheetService.submitDraftTimesheet = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.submitDraftTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetSubmitActions.submitDraftTimesheet({ id: 1 }));
    expect(emitted).toEqual(TimesheetSubmitActions.submitDraftTimesheetFailure({ error: 'fail' }));
  });

  it('should dispatch loadPendingTimesheetsSuccess on loadPendingTimesheets$', () => {
    const timesheets = [{
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    }];
    timesheetService.getPendingTimesheets = () => of(timesheets);
    let emitted;
    effects.loadPendingTimesheets$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.loadPendingTimesheets());
    expect(emitted).toEqual(TimesheetActions.loadPendingTimesheetsSuccess({ timesheets }));
  });

  it('should dispatch loadPendingTimesheetsFailure on error', () => {
    timesheetService.getPendingTimesheets = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.loadPendingTimesheets$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.loadPendingTimesheets());
    expect(emitted).toEqual(TimesheetActions.loadPendingTimesheetsFailure({ error: 'fail' }));
  });

  it('should dispatch submitTimesheetSuccess and loadEmployeeTimesheets on submitTimesheet$', () => {
    const timesheet = {
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    };
    timesheetService.submitTimesheet = () => of(timesheet);
    let results: any[] = [];
    effects.submitTimesheet$.subscribe(result => results.push(result));
    actions$.next(TimesheetActions.submitTimesheet({ request: {} as any }));
    expect(results[0]).toEqual(TimesheetActions.submitTimesheetSuccess({ timesheet }));
    expect(results[1]).toEqual(TimesheetActions.loadEmployeeTimesheets({ employeeId: 1 }));
  });

  it('should dispatch submitTimesheetFailure on error', () => {
    timesheetService.submitTimesheet = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.submitTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.submitTimesheet({ request: {} as any }));
    expect(emitted).toEqual(TimesheetActions.submitTimesheetFailure({ error: 'fail' }));
  });

  it('should dispatch updateTimesheetSuccess on updateTimesheet$', () => {
    const timesheet = {
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    };
    timesheetService.updateTimesheet = () => of(timesheet);
    let emitted;
    effects.updateTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.updateTimesheet({ id: 1, request: {} as any }));
    expect(emitted).toEqual(TimesheetActions.updateTimesheetSuccess({ timesheet }));
  });

  it('should dispatch updateTimesheetFailure on error', () => {
    timesheetService.updateTimesheet = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.updateTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.updateTimesheet({ id: 1, request: {} as any }));
    expect(emitted).toEqual(TimesheetActions.updateTimesheetFailure({ error: 'fail' }));
  });

  it('should dispatch deleteTimesheetSuccess on deleteTimesheet$', () => {
    timesheetService.deleteTimesheet = () => of({});
    let emitted;
    effects.deleteTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.deleteTimesheet({ id: 1 }));
    expect(emitted).toEqual(TimesheetActions.deleteTimesheetSuccess({ id: 1 }));
  });

  it('should dispatch deleteTimesheetFailure on error', () => {
    timesheetService.deleteTimesheet = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.deleteTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.deleteTimesheet({ id: 1 }));
    expect(emitted).toEqual(TimesheetActions.deleteTimesheetFailure({ error: 'fail' }));
  });

  it('should dispatch reviewTimesheetSuccess on reviewTimesheet$', () => {
    const timesheet = {
      id: 1,
      employeeId: 1,
      projectCodeId: 1,
      date: new Date(),
      hoursWorked: 8,
      description: 'desc',
      status: 'Draft' as import('../../core/models/timesheet.model').TimesheetStatus,
      createdAt: new Date(),
      createdBy: 1
    };
    timesheetService.reviewTimesheet = () => of(timesheet);
    let emitted;
    effects.reviewTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.reviewTimesheet({ id: 1, review: {} as any }));
    expect(emitted).toEqual(TimesheetActions.reviewTimesheetSuccess({ timesheet }));
  });

  it('should dispatch reviewTimesheetFailure on error', () => {
    timesheetService.reviewTimesheet = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.reviewTimesheet$.subscribe(result => emitted = result);
    actions$.next(TimesheetActions.reviewTimesheet({ id: 1, review: {} as any }));
    expect(emitted).toEqual(TimesheetActions.reviewTimesheetFailure({ error: 'fail' }));
  });
});
