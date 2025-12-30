import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, mergeMap, withLatestFrom } from 'rxjs/operators';
import { TimesheetService } from '../../core/services/timesheet.service';
import * as TimesheetActions from '../actions/timesheet.actions';
import * as TimesheetSubmitActions from '../actions/timesheet.submit.actions';
import * as AuthSelectors from '../selectors/auth.selectors';
import { Store as NgRxStore } from '@ngrx/store';

/**
 * Timesheet effects for handling side effects
 */
@Injectable()
export class TimesheetEffects {
  private actions$ = inject(Actions);
  private timesheetService = inject(TimesheetService);

  private store = inject(NgRxStore<any>);
  /**
   * Submit draft timesheet effect
   */
  submitDraftTimesheet$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetSubmitActions.submitDraftTimesheet),
      withLatestFrom(this.store.select(AuthSelectors.selectCurrentUser)),
      exhaustMap(([action, user]) =>
        this.timesheetService.submitDraftTimesheet(action.id).pipe(
          mergeMap(timesheet => [
            TimesheetSubmitActions.submitDraftTimesheetSuccess({ timesheet }),
            TimesheetActions.loadEmployeeTimesheets({ employeeId: user?.id ?? 0 })
          ]),
          catchError(error => of(TimesheetSubmitActions.submitDraftTimesheetFailure({ error: error.message })))
        )
      )
    )
  );

  /**
   * Load pending timesheets effect
   */
  loadPendingTimesheets$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.loadPendingTimesheets),
      exhaustMap(() =>
        this.timesheetService.getPendingTimesheets().pipe(
          map(timesheets => TimesheetActions.loadPendingTimesheetsSuccess({ timesheets })),
          catchError(error => of(TimesheetActions.loadPendingTimesheetsFailure({ error: error.message })))
        )
      )
    )
  );

  /**
   * Submit timesheet effect
   */
  submitTimesheet$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.submitTimesheet),
      withLatestFrom(this.store.select(AuthSelectors.selectCurrentUser)),
      exhaustMap(([action, user]) =>
        this.timesheetService.submitTimesheet(action.request).pipe(
          mergeMap(timesheet => [
            TimesheetActions.submitTimesheetSuccess({ timesheet }),
            TimesheetActions.loadEmployeeTimesheets({ employeeId: user?.id ?? 0 })
          ]),
          catchError(error => of(TimesheetActions.submitTimesheetFailure({ error: error.message })))
        )
      )
    )
  );

  /**
   * Update timesheet effect
   */
  updateTimesheet$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.updateTimesheet),
      exhaustMap(action =>
        this.timesheetService.updateTimesheet(action.id, action.request).pipe(
          map(timesheet => TimesheetActions.updateTimesheetSuccess({ timesheet })),
          catchError(error => of(TimesheetActions.updateTimesheetFailure({ error: error.message })))
        )
      )
    )
  );

  /**
   * Delete timesheet effect
   */
  deleteTimesheet$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.deleteTimesheet),
      exhaustMap(action =>
        this.timesheetService.deleteTimesheet(action.id).pipe(
          map(() => TimesheetActions.deleteTimesheetSuccess({ id: action.id })),
          catchError(error => of(TimesheetActions.deleteTimesheetFailure({ error: error.message })))
        )
      )
    )
  );

  /**
   * Load employee timesheets effect
   */
  loadEmployeeTimesheets$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.loadEmployeeTimesheets),
      exhaustMap(action =>
        this.timesheetService
          .getEmployeeTimesheets(action.employeeId, action.startDate, action.endDate)
          .pipe(
            map(timesheets =>
              TimesheetActions.loadEmployeeTimesheetsSuccess({ timesheets })
            ),
            catchError(error => {
              const duplicateMsg = 'already exists for this project and date';
              let errorMessage = extractErrorMessage(error, duplicateMsg) || 'Internal server error. Please try again later.';
              return of(TimesheetActions.loadEmployeeTimesheetsFailure({ error: errorMessage }));
            })
          )
      )
    )
  );

  /**
   * Review timesheet effect (approve/reject)
   */
  reviewTimesheet$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TimesheetActions.reviewTimesheet),
      exhaustMap(action =>
        this.timesheetService.reviewTimesheet(action.id, action.review).pipe(
          map(timesheet =>
            TimesheetActions.reviewTimesheetSuccess({ timesheet })
          ),
          catchError(error =>
            of(TimesheetActions.reviewTimesheetFailure({ error: error.message }))
          )
        )
      )
    )
  );
}

// --- Utility function for error extraction ---
/**
 * Extracts a user-friendly error message, prioritizing duplicate entry errors.
 */
export function extractErrorMessage(error: any, duplicateMsg: string): string | undefined {
  // Check for duplicate error in various possible locations
  if (error?.error) {
    if (typeof error.error === 'string' && error.error.includes(duplicateMsg)) {
      return 'A timesheet entry already exists for this project and date.';
    }
    if (typeof error.error === 'object') {
      if (typeof error.error.error === 'string' && error.error.error.includes(duplicateMsg)) {
        return 'A timesheet entry already exists for this project and date.';
      }
      if (typeof error.error.message === 'string') {
        if (error.error.message.includes(duplicateMsg)) {
          return 'A timesheet entry already exists for this project and date.';
        }
        return error.error.message;
      }
    }
  }
  if (typeof error?.message === 'string') {
    if (error.message.includes(duplicateMsg)) {
      return 'A timesheet entry already exists for this project and date.';
    }
    return error.message;
  }
  return undefined;
}
