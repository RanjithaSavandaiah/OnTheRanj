import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState } from '../reducers/auth.reducer';

/**
 * Authentication selectors for accessing auth state
 */

// Feature selector
export const selectAuthState = createFeatureSelector<AuthState>('auth');

// Select current user
export const selectCurrentUser = createSelector(
  selectAuthState,
  (state: AuthState | undefined) => state?.user ?? null
);

// Select authentication status
export const selectIsAuthenticated = createSelector(
  selectAuthState,
  (state: AuthState | undefined) => state?.isAuthenticated ?? false
);

// Select token
export const selectToken = createSelector(
  selectAuthState,
  (state: AuthState | undefined) => state?.token ?? null
);

// Select loading state
export const selectAuthLoading = createSelector(
  selectAuthState,
  (state: AuthState | undefined) => state?.loading ?? false
);

// Select error
export const selectAuthError = createSelector(
  selectAuthState,
  (state: AuthState | undefined) => state?.error ?? null
);

// Select user role
export const selectUserRole = createSelector(
  selectCurrentUser,
  (user) => user?.role
);

// Check if user is manager
export const selectIsManager = createSelector(
  selectUserRole,
  (role) => role === 'Manager'
);

// Check if user is employee
export const selectIsEmployee = createSelector(
  selectUserRole,
  (role) => role === 'Employee'
);
