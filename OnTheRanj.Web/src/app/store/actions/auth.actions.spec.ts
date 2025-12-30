import * as AuthActions from './auth.actions';
import { LoginRequest, RegisterRequest, User } from '../../core/models/user.model';

describe('Auth Actions', () => {
  it('should create login action', () => {
    const credentials: LoginRequest = { email: 'test@example.com', password: 'pass' };
    const action = AuthActions.login({ credentials });
    expect(action.type).toBe('[Auth] Login');
    expect(action.credentials).toEqual(credentials);
  });

  it('should create loginSuccess action', () => {
    const user: User = { id: 1, email: 'test@example.com', fullName: 'Test', role: 'Employee', isActive: true };
    const token = 'token123';
    const action = AuthActions.loginSuccess({ user, token });
    expect(action.type).toBe('[Auth] Login Success');
    expect(action.user).toEqual(user);
    expect(action.token).toBe(token);
  });

  it('should create loginFailure action', () => {
    const error = 'Invalid credentials';
    const action = AuthActions.loginFailure({ error });
    expect(action.type).toBe('[Auth] Login Failure');
    expect(action.error).toBe(error);
  });

  it('should create register action', () => {
    const request: RegisterRequest = { email: 'test@example.com', password: 'pass', fullName: 'Test', role: 'Employee' };
    const action = AuthActions.register({ request });
    expect(action.type).toBe('[Auth] Register');
    expect(action.request).toEqual(request);
  });

  it('should create registerSuccess action', () => {
    const message = 'Registered!';
    const action = AuthActions.registerSuccess({ message });
    expect(action.type).toBe('[Auth] Register Success');
    expect(action.message).toBe(message);
  });

  it('should create registerFailure action', () => {
    const error = 'Email exists';
    const action = AuthActions.registerFailure({ error });
    expect(action.type).toBe('[Auth] Register Failure');
    expect(action.error).toBe(error);
  });

  it('should create logout action', () => {
    const action = AuthActions.logout();
    expect(action.type).toBe('[Auth] Logout');
  });

  it('should create loadUser action', () => {
    const action = AuthActions.loadUser();
    expect(action.type).toBe('[Auth] Load User');
  });
});
