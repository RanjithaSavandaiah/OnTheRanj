import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Timesheet } from '../../core/models/timesheet.model';
import * as TimesheetActions from '../actions/timesheet.actions';
import * as TimesheetSubmitActions from '../actions/timesheet.submit.actions';

/**
 * Timesheet state interface using NgRx Entity
 */
export interface TimesheetState extends EntityState<Timesheet> {
  selectedTimesheetId: number | null;
  loading: boolean;
  error: string | null;
}

/**
 * Entity adapter for timesheet management
 */
export const timesheetAdapter: EntityAdapter<Timesheet> = createEntityAdapter<Timesheet>();

/**
 * Initial timesheet state
 */
export const initialState: TimesheetState = timesheetAdapter.getInitialState({
  selectedTimesheetId: null,
  loading: false,
  error: null
});

/**
 * Timesheet reducer
 */
export const timesheetReducer = createReducer(
  initialState,

  // Load employee timesheets
  on(TimesheetActions.loadEmployeeTimesheets, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.loadEmployeeTimesheetsSuccess, (state, { timesheets }) =>
    timesheetAdapter.setAll(timesheets, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(TimesheetActions.loadEmployeeTimesheetsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Load pending timesheets
  on(TimesheetActions.loadPendingTimesheets, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.loadPendingTimesheetsSuccess, (state, { timesheets }) =>
    timesheetAdapter.setAll(timesheets, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(TimesheetActions.loadPendingTimesheetsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Submit timesheet
  on(TimesheetActions.submitTimesheet, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.submitTimesheetSuccess, (state, { timesheet }) =>
    timesheetAdapter.addOne(timesheet, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(TimesheetActions.submitTimesheetFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Update timesheet
  on(TimesheetActions.updateTimesheet, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.updateTimesheetSuccess, (state, { timesheet }) =>
    timesheetAdapter.updateOne(
      { id: timesheet.id, changes: timesheet },
      {
        ...state,
        loading: false,
        error: null
      }
    )
  ),

  on(TimesheetActions.updateTimesheetFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Delete timesheet
  on(TimesheetActions.deleteTimesheet, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.deleteTimesheetSuccess, (state, { id }) =>
    timesheetAdapter.removeOne(id, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(TimesheetActions.deleteTimesheetFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Review timesheet
  on(TimesheetActions.reviewTimesheet, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetActions.reviewTimesheetSuccess, (state, { timesheet }) =>
    timesheetAdapter.updateOne(
      { id: timesheet.id, changes: timesheet },
      {
        ...state,
        loading: false,
        error: null
      }
    )
  ),
  // Submit draft timesheet
  on(TimesheetSubmitActions.submitDraftTimesheet, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(TimesheetSubmitActions.submitDraftTimesheetSuccess, (state, { timesheet }) =>
    timesheetAdapter.updateOne(
      { id: timesheet.id, changes: timesheet },
      {
        ...state,
        loading: false,
        error: null
      }
    )
  ),

  on(TimesheetSubmitActions.submitDraftTimesheetFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  on(TimesheetActions.reviewTimesheetFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);

/**
 * Export entity selectors
 */
export const {
  selectIds: selectTimesheetIds,
  selectEntities: selectTimesheetEntities,
  selectAll: selectAllTimesheets,
  selectTotal: selectTimesheetTotal
} = timesheetAdapter.getSelectors();
