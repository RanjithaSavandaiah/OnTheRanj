import * as TimesheetActions from './timesheet.actions';
import { Timesheet, TimesheetRequest, TimesheetReviewRequest } from '../../core/models/timesheet.model';

describe('Timesheet Actions', () => {
  it('should create loadEmployeeTimesheets action', () => {
    const action = TimesheetActions.loadEmployeeTimesheets({ employeeId: 1 });
    expect(action.type).toBe('[Timesheet] Load Employee Timesheets');
    expect(action.employeeId).toBe(1);
  });

  it('should create loadEmployeeTimesheetsSuccess action', () => {
    const timesheets: Timesheet[] = [{ id: 1, employeeId: 1, projectCodeId: 1, date: new Date('2025-12-27'), hoursWorked: 8, description: 'Worked', status: 'Submitted', createdAt: new Date() }];
    const action = TimesheetActions.loadEmployeeTimesheetsSuccess({ timesheets });
    expect(action.type).toBe('[Timesheet] Load Employee Timesheets Success');
    expect(action.timesheets).toEqual(timesheets);
  });

  it('should create loadEmployeeTimesheetsFailure action', () => {
    const error = 'Failed';
    const action = TimesheetActions.loadEmployeeTimesheetsFailure({ error });
    expect(action.type).toBe('[Timesheet] Load Employee Timesheets Failure');
    expect(action.error).toBe(error);
  });

  it('should create submitTimesheet action', () => {
    const request: TimesheetRequest = { employeeId: 1, projectCodeId: 1, date: '2025-12-27', hoursWorked: 8, description: 'Worked' };
    const action = TimesheetActions.submitTimesheet({ request });
    expect(action.type).toBe('[Timesheet] Submit Timesheet');
    expect(action.request).toEqual(request);
  });

  it('should create submitTimesheetSuccess action', () => {
    const timesheet: Timesheet = { id: 1, employeeId: 1, projectCodeId: 1, date: new Date('2025-12-27'), hoursWorked: 8, description: 'Worked', status: 'Submitted', createdAt: new Date() };
    const action = TimesheetActions.submitTimesheetSuccess({ timesheet });
    expect(action.type).toBe('[Timesheet] Submit Timesheet Success');
    expect(action.timesheet).toEqual(timesheet);
  });

  it('should create submitTimesheetFailure action', () => {
    const error = 'Failed';
    const action = TimesheetActions.submitTimesheetFailure({ error });
    expect(action.type).toBe('[Timesheet] Submit Timesheet Failure');
    expect(action.error).toBe(error);
  });

  it('should create reviewTimesheet action', () => {
    const review: TimesheetReviewRequest = { comments: 'ok' };
    const action = TimesheetActions.reviewTimesheet({ id: 1, review });
    expect(action.type).toBe('[Timesheet] Review Timesheet');
    expect(action.id).toBe(1);
    expect(action.review).toEqual(review);
  });

  it('should create reviewTimesheetSuccess action', () => {
    const timesheet: Timesheet = { id: 1, employeeId: 1, projectCodeId: 1, date: new Date('2025-12-27'), hoursWorked: 8, description: 'Worked', status: 'Approved', createdAt: new Date() };
    const action = TimesheetActions.reviewTimesheetSuccess({ timesheet });
    expect(action.type).toBe('[Timesheet] Review Timesheet Success');
    expect(action.timesheet).toEqual(timesheet);
  });

  it('should create reviewTimesheetFailure action', () => {
    const error = 'Failed';
    const action = TimesheetActions.reviewTimesheetFailure({ error });
    expect(action.type).toBe('[Timesheet] Review Timesheet Failure');
    expect(action.error).toBe(error);
  });
});
