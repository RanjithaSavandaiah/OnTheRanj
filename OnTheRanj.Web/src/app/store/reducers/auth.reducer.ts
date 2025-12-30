import { createReducer, on } from '@ngrx/store';
import { User } from '../../core/models/user.model';
import * as AuthActions from '../actions/auth.actions';

/**
 * Authentication state interface
 */
export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

/**
 * Initial authentication state
 */
export const initialState: AuthState = {
  user: null,
  token: null,
  isAuthenticated: false,
  loading: false,
  error: null
};

/**
 * Authentication reducer
 */
export const authReducer = createReducer(
  initialState,

  // Login
  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(AuthActions.loginSuccess, (state, { user, token }) => ({
    ...state,
    user,
    token,
    isAuthenticated: true,
    loading: false,
    error: null
  })),

  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Register
  on(AuthActions.register, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(AuthActions.registerSuccess, (state) => ({
    ...state,
    loading: false,
    error: null
  })),

  on(AuthActions.registerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Logout
  on(AuthActions.logout, () => initialState)
);
