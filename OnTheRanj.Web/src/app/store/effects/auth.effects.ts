import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, exhaustMap, tap } from 'rxjs/operators';
import { AuthService } from '../../core/services/auth.service';
import * as AuthActions from '../actions/auth.actions';

/**
 * Authentication effects for handling side effects
 */
@Injectable()
export class AuthEffects {
  // Load user from token on app startup
  loadUser$ = createEffect(() => {
    const actions$ = inject(Actions);
    return actions$.pipe(
      ofType(AuthActions.loadUser),
      map(() => {
        const token = localStorage.getItem('token');
        if (token) {
          const payload = this.parseJwt(token);
          if (payload) {
            return AuthActions.loginSuccess({
              user: {
                id: parseInt(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid),
                fullName: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || payload.unique_name,
                email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || payload.email,
                role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role,
                isActive: true
              },
              token
            });
          }
        }
        return { type: '[Auth] No User Loaded' };
      })
    );
  });
  login$ = createEffect(() => {
    const actions$ = inject(Actions);
    const authService = inject(AuthService);
    return actions$.pipe(
      ofType(AuthActions.login),
      exhaustMap(action =>
        authService.login(action.credentials).pipe(
          map(response => {
            // Parse user info from JWT token
            const payload = this.parseJwt(response.token);
            return AuthActions.loginSuccess({
              user: {
                id: parseInt(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid),
                fullName: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || payload.unique_name,
                email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || payload.email,
                role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role,
                isActive: true
              },
              token: response.token
            });
          }),
          catchError(error =>
            of(AuthActions.loginFailure({ error: error.message }))
          )
        )
      )
    );
  });

  /**
   * Login success effect - redirects to appropriate dashboard
   */
  loginSuccess$ = createEffect(
    () => {
      const actions$ = inject(Actions);
      const router = inject(Router);
      return actions$.pipe(
        ofType(AuthActions.loginSuccess),
        tap(({ user }) => {
          // Redirect based on user role
          if (user.role === 'Manager') {
            router.navigate(['/manager/dashboard']);
          } else {
            router.navigate(['/employee/timesheets']);
          }
        })
      );
    },
    { dispatch: false }
  );

  /**
   * Register effect - handles registration requests
   */
  register$ = createEffect(() => {
    const actions$ = inject(Actions);
    const authService = inject(AuthService);
    return actions$.pipe(
      ofType(AuthActions.register),
      exhaustMap(action =>
        authService.register(action.request).pipe(
          map(response =>
            AuthActions.registerSuccess({ message: response.message })
          ),
          catchError(error =>
            of(AuthActions.registerFailure({ error: error.message }))
          )
        )
      )
    );
  });

  /**
   * Register success effect - redirects to login
   */
  registerSuccess$ = createEffect(
    () => {
      const actions$ = inject(Actions);
      const router = inject(Router);
      return actions$.pipe(
        ofType(AuthActions.registerSuccess),
        tap(() => {
          router.navigate(['/login']);
        })
      );
    },
    { dispatch: false }
  );

  /**
   * Logout effect - handles logout
   */
  logout$ = createEffect(
    () => {
      const actions$ = inject(Actions);
      const authService = inject(AuthService);
      return actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          authService.logout();
        })
      );
    },
    { dispatch: false }
  );

  constructor() {}

  /**
   * Parse JWT token to extract payload
   */
  private parseJwt(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }
}
