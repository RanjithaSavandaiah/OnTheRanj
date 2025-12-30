import { createAction, props } from '@ngrx/store';

export const submitDraftTimesheet = createAction(
  '[Timesheet] Submit Draft Timesheet',
  props<{ id: number }>()
);

export const submitDraftTimesheetSuccess = createAction(
  '[Timesheet] Submit Draft Timesheet Success',
  props<{ timesheet: any }>()
);

export const submitDraftTimesheetFailure = createAction(
  '[Timesheet] Submit Draft Timesheet Failure',
  props<{ error: string }>()
);
