import * as TimesheetSelectors from './timesheet.selectors';
import { TimesheetState } from '../reducers/timesheet.reducer';
import { Timesheet } from '../../core/models/timesheet.model';

describe('Timesheet Selectors', () => {
  const timesheet: Timesheet = { id: 1, employeeId: 1, projectCodeId: 1, date: new Date('2025-12-27'), hoursWorked: 8, description: 'Worked', status: 'Submitted' as 'Submitted', createdAt: new Date() };
  const state: { timesheet: TimesheetState } = {
    timesheet: {
      ids: [1],
      entities: { 1: timesheet },
      selectedTimesheetId: 1,
      loading: false,
      error: null
    }
  } as any;

  it('should select timesheet state', () => {
    expect(TimesheetSelectors.selectTimesheetState(state)).toEqual(state.timesheet);
  });

  it('should select all timesheets', () => {
    expect(TimesheetSelectors.selectTimesheets.projector(state.timesheet)).toEqual([timesheet]);
  });

  it('should select timesheet entities', () => {
    expect(TimesheetSelectors.selectTimesheetEntitiesSelector.projector(state.timesheet)).toEqual({ 1: timesheet });
  });

  it('should select timesheets dictionary', () => {
    expect(TimesheetSelectors.selectTimesheetsDictionary.projector(state.timesheet)).toEqual({ 1: timesheet });
  });

  it('should select selected timesheet id', () => {
    expect(TimesheetSelectors.selectSelectedTimesheetId.projector(state.timesheet)).toBe(1);
  });

  it('should select selected timesheet', () => {
    expect(TimesheetSelectors.selectSelectedTimesheet.projector({ 1: timesheet }, 1)).toEqual(timesheet);
    expect(TimesheetSelectors.selectSelectedTimesheet.projector({ 1: timesheet }, null)).toBeNull();
  });

  it('should select loading', () => {
    expect(TimesheetSelectors.selectTimesheetLoading.projector(state.timesheet)).toBe(false);
  });

  it('should select error', () => {
    expect(TimesheetSelectors.selectTimesheetError.projector(state.timesheet)).toBeNull();
  });

  it('should select pending timesheets', () => {
    expect(TimesheetSelectors.selectPendingTimesheets.projector([timesheet])).toEqual([timesheet]);
  });

  it('should select approved timesheets', () => {
    const approved = { ...timesheet, status: 'Approved' as 'Approved' };
    expect(TimesheetSelectors.selectApprovedTimesheets.projector([approved])).toEqual([approved]);
  });

  it('should select rejected timesheets', () => {
    const rejected = { ...timesheet, status: 'Rejected' as 'Rejected' };
    expect(TimesheetSelectors.selectRejectedTimesheets.projector([rejected])).toEqual([rejected]);
  });

  it('should select total hours', () => {
    expect(TimesheetSelectors.selectTotalHours.projector([timesheet])).toBe(timesheet.hoursWorked);
  });

  it('should select timesheet by id', () => {
    expect(TimesheetSelectors.selectTimesheetById(1).projector({ 1: timesheet })).toEqual(timesheet);
  });

  it('should select timesheets by status', () => {
    expect(TimesheetSelectors.selectTimesheetsByStatus('Submitted').projector([timesheet])).toEqual([timesheet]);
  });
});
