import { timesheetReducer, initialState } from './timesheet.reducer';
import * as TimesheetActions from '../actions/timesheet.actions';
import * as TimesheetSubmitActions from '../actions/timesheet.submit.actions';
import { Timesheet } from '../../core/models/timesheet.model';

describe('Timesheet Reducer', () => {
  const timesheet: Timesheet = { id: 1, employeeId: 1, projectCodeId: 1, date: new Date('2025-12-27'), hoursWorked: 8, description: 'Worked', status: 'Submitted' as 'Submitted', createdAt: new Date() };

  it('should return initial state', () => {
    expect(timesheetReducer(undefined, { type: '@@init' } as any)).toEqual(initialState);
  });

  it('should set loading true on loadEmployeeTimesheets', () => {
    const state = timesheetReducer(initialState, TimesheetActions.loadEmployeeTimesheets({ employeeId: 1 }));
    expect(state.loading).toBe(true);
    expect(state.error).toBeNull();
  });

  it('should set all timesheets on loadEmployeeTimesheetsSuccess', () => {
    const state = timesheetReducer(initialState, TimesheetActions.loadEmployeeTimesheetsSuccess({ timesheets: [timesheet] }));
    expect(state.ids).toContain(1);
    expect(state.entities[1]).toEqual(timesheet);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should add timesheet on submitTimesheetSuccess', () => {
    const state = timesheetReducer(initialState, TimesheetActions.submitTimesheetSuccess({ timesheet }));
    expect(state.entities[1]).toEqual(timesheet);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should update timesheet on updateTimesheetSuccess', () => {
    const updated = { ...timesheet, hoursWorked: 10 };
    const state = timesheetReducer({ ...initialState, entities: { 1: timesheet }, ids: [1] }, TimesheetActions.updateTimesheetSuccess({ timesheet: updated }));
    expect(state.entities[1]).toEqual(updated);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should remove timesheet on deleteTimesheetSuccess', () => {
    const prev = timesheetReducer(initialState, TimesheetActions.submitTimesheetSuccess({ timesheet }));
    const state = timesheetReducer(prev, TimesheetActions.deleteTimesheetSuccess({ id: 1 }));
    expect(state.entities[1]).toBeUndefined();
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should update timesheet on submitDraftTimesheetSuccess', () => {
    const updated = { ...timesheet, status: 'Submitted' as 'Submitted' };
    const state = timesheetReducer({ ...initialState, entities: { 1: timesheet }, ids: [1] }, TimesheetSubmitActions.submitDraftTimesheetSuccess({ timesheet: updated }));
    expect(state.entities[1]).toEqual(updated);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });
});
