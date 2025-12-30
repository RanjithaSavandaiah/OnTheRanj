import * as TimesheetSubmitActions from './timesheet.submit.actions';

describe('Timesheet Submit Actions', () => {
  it('should create submitDraftTimesheet action', () => {
    const action = TimesheetSubmitActions.submitDraftTimesheet({ id: 1 });
    expect(action.type).toBe('[Timesheet] Submit Draft Timesheet');
    expect(action.id).toBe(1);
  });

  it('should create submitDraftTimesheetSuccess action', () => {
    const timesheet = { id: 1 };
    const action = TimesheetSubmitActions.submitDraftTimesheetSuccess({ timesheet });
    expect(action.type).toBe('[Timesheet] Submit Draft Timesheet Success');
    expect(action.timesheet).toEqual(timesheet);
  });

  it('should create submitDraftTimesheetFailure action', () => {
    const error = 'Failed';
    const action = TimesheetSubmitActions.submitDraftTimesheetFailure({ error });
    expect(action.type).toBe('[Timesheet] Submit Draft Timesheet Failure');
    expect(action.error).toBe(error);
  });
});
