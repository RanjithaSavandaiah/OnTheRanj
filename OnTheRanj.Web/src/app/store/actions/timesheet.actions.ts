import { createAction, props } from '@ngrx/store';
import { Timesheet, TimesheetRequest, TimesheetReviewRequest } from '../../core/models/timesheet.model';

/**
 * Timesheet actions for NgRx store
 */

// Load employee timesheets
export const loadEmployeeTimesheets = createAction(
  '[Timesheet] Load Employee Timesheets',
  props<{ employeeId: number; startDate?: Date; endDate?: Date }>()
);

export const loadEmployeeTimesheetsSuccess = createAction(
  '[Timesheet] Load Employee Timesheets Success',
  props<{ timesheets: Timesheet[] }>()
);

export const loadEmployeeTimesheetsFailure = createAction(
  '[Timesheet] Load Employee Timesheets Failure',
  props<{ error: string }>()
);

// Load pending timesheets (for managers)
export const loadPendingTimesheets = createAction('[Timesheet] Load Pending Timesheets');

export const loadPendingTimesheetsSuccess = createAction(
  '[Timesheet] Load Pending Timesheets Success',
  props<{ timesheets: Timesheet[] }>()
);

export const loadPendingTimesheetsFailure = createAction(
  '[Timesheet] Load Pending Timesheets Failure',
  props<{ error: string }>()
);

// Submit timesheet
export const submitTimesheet = createAction(
  '[Timesheet] Submit Timesheet',
  props<{ request: TimesheetRequest }>()
);

export const submitTimesheetSuccess = createAction(
  '[Timesheet] Submit Timesheet Success',
  props<{ timesheet: Timesheet }>()
);

export const submitTimesheetFailure = createAction(
  '[Timesheet] Submit Timesheet Failure',
  props<{ error: string }>()
);

// Update timesheet
export const updateTimesheet = createAction(
  '[Timesheet] Update Timesheet',
  props<{ id: number; request: TimesheetRequest }>()
);

export const updateTimesheetSuccess = createAction(
  '[Timesheet] Update Timesheet Success',
  props<{ timesheet: Timesheet }>()
);

export const updateTimesheetFailure = createAction(
  '[Timesheet] Update Timesheet Failure',
  props<{ error: string }>()
);

// Delete timesheet
export const deleteTimesheet = createAction(
  '[Timesheet] Delete Timesheet',
  props<{ id: number }>()
);

export const deleteTimesheetSuccess = createAction(
  '[Timesheet] Delete Timesheet Success',
  props<{ id: number }>()
);

export const deleteTimesheetFailure = createAction(
  '[Timesheet] Delete Timesheet Failure',
  props<{ error: string }>()
);

// Review timesheet (approve/reject)
export const reviewTimesheet = createAction(
  '[Timesheet] Review Timesheet',
  props<{ id: number; review: TimesheetReviewRequest }>()
);

export const reviewTimesheetSuccess = createAction(
  '[Timesheet] Review Timesheet Success',
  props<{ timesheet: Timesheet }>()
);

export const reviewTimesheetFailure = createAction(
  '[Timesheet] Review Timesheet Failure',
  props<{ error: string }>()
);
