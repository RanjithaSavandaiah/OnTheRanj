import { ActionReducerMap } from '@ngrx/store';
import { AuthState, authReducer } from './reducers/auth.reducer';
import { ProjectState, projectReducer } from './reducers/project.reducer';
import { TimesheetState, timesheetReducer } from './reducers/timesheet.reducer';

/**
 * Application state interface
 */
export interface AppState {
  auth: AuthState;
  project: ProjectState;
  timesheet: TimesheetState;
}

/**
 * Root reducer map
 */
export const reducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  project: projectReducer,
  timesheet: timesheetReducer
};
