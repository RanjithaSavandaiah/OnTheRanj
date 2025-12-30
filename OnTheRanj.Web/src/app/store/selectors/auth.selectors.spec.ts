import * as AuthSelectors from './auth.selectors';
import { AuthState } from '../reducers/auth.reducer';

describe('Auth Selectors', () => {
  const user = { id: 1, fullName: 'Test', email: 'test@example.com', role: 'Manager' as 'Manager', isActive: true };
  const state: { auth: AuthState } = {
    auth: {
      user,
      token: 'token',
      isAuthenticated: true,
      loading: false,
      error: null
    }
  };

  it('should select auth state', () => {
    expect(AuthSelectors.selectAuthState(state)).toEqual(state.auth);
  });

  it('should select current user', () => {
    expect(AuthSelectors.selectCurrentUser.projector(state.auth)).toEqual(user);
  });

  it('should select isAuthenticated', () => {
    expect(AuthSelectors.selectIsAuthenticated.projector(state.auth)).toBe(true);
  });

  it('should select token', () => {
    expect(AuthSelectors.selectToken.projector(state.auth)).toBe('token');
  });

  it('should select loading', () => {
    expect(AuthSelectors.selectAuthLoading.projector(state.auth)).toBe(false);
  });

  it('should select error', () => {
    expect(AuthSelectors.selectAuthError.projector(state.auth)).toBeNull();
  });

  it('should select user role', () => {
    expect(AuthSelectors.selectUserRole.projector(user)).toBe('Manager');
  });

  it('should select isManager', () => {
    expect(AuthSelectors.selectIsManager.projector('Manager')).toBe(true);
    expect(AuthSelectors.selectIsManager.projector('Employee')).toBe(false);
  });

  it('should select isEmployee', () => {
    expect(AuthSelectors.selectIsEmployee.projector('Employee')).toBe(true);
    expect(AuthSelectors.selectIsEmployee.projector('Manager')).toBe(false);
  });
});
