import { createAction, props } from '@ngrx/store';
import { User, LoginRequest, RegisterRequest } from '../../core/models/user.model';

/**
 * Authentication actions for NgRx store
 */

// Login actions
export const login = createAction(
  '[Auth] Login',
  props<{ credentials: LoginRequest }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: User; token: string }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);

// Register actions
export const register = createAction(
  '[Auth] Register',
  props<{ request: RegisterRequest }>()
);

export const registerSuccess = createAction(
  '[Auth] Register Success',
  props<{ message: string }>()
);

export const registerFailure = createAction(
  '[Auth] Register Failure',
  props<{ error: string }>()
);

// Logout action
export const logout = createAction('[Auth] Logout');

// Load user from token
export const loadUser = createAction('[Auth] Load User');
