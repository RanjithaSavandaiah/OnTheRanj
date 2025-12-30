import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

/**
 * Global error interceptor for HTTP requests
 * Handles common error scenarios and provides user-friendly messages
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side error
        switch (error.status) {
          case 400:
            errorMessage = error.error?.message || 'Bad request';
            break;
          case 401:
            errorMessage = 'Unauthorized. Please login again.';
            router.navigate(['/login']);
            break;
          case 403:
            errorMessage = 'Access forbidden. You don\'t have permission.';
            break;
          case 404:
            errorMessage = 'Resource not found';
            break;
          case 500:
            // Custom handling for duplicate timesheet error
            if (
              typeof error.error?.message === 'string' &&
              error.error.message.includes('already exists for this project and date')
            ) {
              errorMessage = 'A timesheet for this project and date already exists. Please choose a different date or project.';
            } else {
              errorMessage = 'Internal server error. Please try again later.';
            }
            break;
          default:
            errorMessage = `Error ${error.status}: ${error.message}`;
        }
      }

      // Show user-friendly error
      snackBar.open(errorMessage, 'Close', { duration: 5000 });

      // Log error for debugging
      console.error('HTTP Error:', {
        status: error.status,
        message: errorMessage,
        url: req.url,
        error: error.error
      });

      return throwError(() => new Error(errorMessage));
    })
  );
};
