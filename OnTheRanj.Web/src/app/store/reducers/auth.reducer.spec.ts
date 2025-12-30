import { authReducer, initialState, AuthState } from './auth.reducer';
import * as AuthActions from '../actions/auth.actions';

describe('Auth Reducer', () => {
  it('should return initial state', () => {
    expect(authReducer(undefined, { type: '@@init' } as any)).toEqual(initialState);
  });

  it('should set loading true on login', () => {
    const state = authReducer(initialState, AuthActions.login({ credentials: { email: 'a', password: 'b' } }));
    expect(state.loading).toBe(true);
    expect(state.error).toBeNull();
  });

  it('should set user/token on loginSuccess', () => {
    const user = { id: 1, fullName: 'Test', email: 'test@example.com', role: 'Manager' as 'Manager', isActive: true };
    const token = 'token';
    const state = authReducer(initialState, AuthActions.loginSuccess({ user, token }));
    expect(state.user).toEqual(user);
    expect(state.token).toBe(token);
    expect(state.isAuthenticated).toBe(true);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should set error on loginFailure', () => {
    const state = authReducer(initialState, AuthActions.loginFailure({ error: 'fail' }));
    expect(state.loading).toBe(false);
    expect(state.error).toBe('fail');
  });

  it('should set loading true on register', () => {
    const state = authReducer(initialState, AuthActions.register({ request: { fullName: 'T', email: 't', password: 'p', role: 'Employee' } }));
    expect(state.loading).toBe(true);
    expect(state.error).toBeNull();
  });

  it('should set loading false on registerSuccess', () => {
    const state = authReducer({ ...initialState, loading: true }, AuthActions.registerSuccess({ message: 'ok' }));
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should set error on registerFailure', () => {
    const state = authReducer(initialState, AuthActions.registerFailure({ error: 'fail' }));
    expect(state.loading).toBe(false);
    expect(state.error).toBe('fail');
  });

  it('should reset state on logout', () => {
    const prev: AuthState = { ...initialState, user: { id: 1, fullName: 'T', email: 't', role: 'Manager' as 'Manager', isActive: true }, token: 'tok', isAuthenticated: true };
    expect(authReducer(prev, AuthActions.logout())).toEqual(initialState);
  });
});
