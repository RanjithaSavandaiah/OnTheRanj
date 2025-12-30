import { errorInterceptor } from './error.interceptor';
import { HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { throwError } from 'rxjs';
import { runInInjectionContext, createEnvironmentInjector, EnvironmentInjector } from '@angular/core';
import { TestBed } from '@angular/core/testing';

describe('errorInterceptor', () => {
  let navigateCalledWith: any[] | null;
  let snackBarOpenCalledWith: any[] | null;
  let router: any;
  let snackBar: any;
  let next: any;
  let injector: EnvironmentInjector;

  beforeEach(() => {
    navigateCalledWith = null;
    snackBarOpenCalledWith = null;
    router = { navigate: (...args: any[]) => { navigateCalledWith = args; } };
    snackBar = { open: (...args: any[]) => { snackBarOpenCalledWith = args; } };
    next = (_req: any) => {};
    injector = createEnvironmentInjector([
      { provide: Router, useValue: router },
      { provide: MatSnackBar, useValue: snackBar }
    ], TestBed.inject(EnvironmentInjector));
  });

  function runInterceptor(req: any, nextFn: any) {
    return runInInjectionContext(injector, () => errorInterceptor(req, nextFn));
  }

  it('should show error for 400', async () => {
    next = () => throwError(() => new HttpErrorResponse({ status: 400, error: { message: 'Bad request' } }));
    await new Promise<void>((resolve) => {
      runInterceptor({} as any, next).subscribe({
        error: (_err: any) => {
          expect(snackBarOpenCalledWith).toEqual(['Bad request', 'Close', { duration: 5000 }]);
          resolve();
        }
      });
    });
  });

  it('should redirect and show error for 401', async () => {
    next = () => throwError(() => new HttpErrorResponse({ status: 401 }));
    await new Promise<void>((resolve) => {
      runInterceptor({ url: '/api' } as any, next).subscribe({
        error: (_err: any) => {
          expect(snackBarOpenCalledWith).toEqual(['Unauthorized. Please login again.', 'Close', { duration: 5000 }]);
          expect(navigateCalledWith).toEqual([['/login']]);
          resolve();
        }
      });
    });
  });

  it('should show error for 404', async () => {
    next = () => throwError(() => new HttpErrorResponse({ status: 404 }));
    await new Promise<void>((resolve) => {
      runInterceptor({ url: '/api' } as any, next).subscribe({
        error: (_err: any) => {
          expect(snackBarOpenCalledWith).toEqual(['Resource not found', 'Close', { duration: 5000 }]);
          resolve();
        }
      });
    });
  });

  it('should show duplicate timesheet error for 500 with specific message', async () => {
    next = () => throwError(() => new HttpErrorResponse({ status: 500, error: { message: 'already exists for this project and date' } }));
    await new Promise<void>((resolve) => {
      runInterceptor({ url: '/api' } as any, next).subscribe({
        error: (_err: any) => {
          expect(snackBarOpenCalledWith).toEqual([
            'A timesheet for this project and date already exists. Please choose a different date or project.',
            'Close',
            { duration: 5000 }
          ]);
          resolve();
        }
      });
    });
  });
});

