import { createFeatureSelector, createSelector } from '@ngrx/store';
import { TimesheetState, selectAllTimesheets, selectTimesheetEntities } from '../reducers/timesheet.reducer';
import { TimesheetStatus } from '../../core/models/timesheet.model';

/**
 * Timesheet selectors for accessing timesheet state
 */

// Feature selector
export const selectTimesheetState = createFeatureSelector<TimesheetState>('timesheet');

// Select all timesheets using entity adapter selector
export const selectTimesheets = createSelector(
  selectTimesheetState,
  selectAllTimesheets
);

// Select timesheet entities (for entity map access)
export const selectTimesheetEntitiesSelector = createSelector(
  selectTimesheetState,
  selectTimesheetEntities
);

// Dictionary of timesheet entities for lookup by ID
export const selectTimesheetsDictionary = createSelector(
  selectTimesheetState,
  selectTimesheetEntities
);

// For compatibility with component usage
export { selectTimesheetEntities } from '../reducers/timesheet.reducer';

// Select selected timesheet ID
export const selectSelectedTimesheetId = createSelector(
  selectTimesheetState,
  (state: TimesheetState) => state.selectedTimesheetId
);

// Select selected timesheet
export const selectSelectedTimesheet = createSelector(
  selectTimesheetsDictionary,
  selectSelectedTimesheetId,
  (entities: { [id: number]: any }, selectedId: number | null) => (selectedId ? entities[selectedId] : null)
);

// Select loading state
export const selectTimesheetLoading = createSelector(
  selectTimesheetState,
  (state: TimesheetState) => state.loading
);

// Select error
export const selectTimesheetError = createSelector(
  selectTimesheetState,
  (state: TimesheetState) => state.error
);

// Select pending timesheets
export const selectPendingTimesheets = createSelector(
  selectTimesheets,
  (timesheets) => timesheets.filter(ts => ts.status === 'Submitted')
);

// Select approved timesheets
export const selectApprovedTimesheets = createSelector(
  selectTimesheets,
  (timesheets) => timesheets.filter(ts => ts.status === 'Approved')
);

// Select rejected timesheets
export const selectRejectedTimesheets = createSelector(
  selectTimesheets,
  (timesheets) => timesheets.filter(ts => ts.status === 'Rejected')
);

// Select total hours
export const selectTotalHours = createSelector(
  selectTimesheets,
  (timesheets) => timesheets.reduce((total, ts) => total + ts.hoursWorked, 0)
);

// Select timesheet by ID
export const selectTimesheetById = (id: number) =>
  createSelector(
    selectTimesheetsDictionary,
    (entities: { [id: number]: any }) => entities[id]
  );

// Select timesheets by status
export const selectTimesheetsByStatus = (status: TimesheetStatus) =>
  createSelector(
    selectTimesheets,
    (timesheets) => timesheets.filter(ts => ts.status === status)
  );
