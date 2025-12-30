import { AuthEffects } from './auth.effects';
import * as AuthActions from '../actions/auth.actions';
import { AuthService } from '../../core/services/auth.service';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable, of, throwError, Subject } from 'rxjs';
import { TestBed } from '@angular/core/testing';
import { provideMockStore } from '@ngrx/store/testing';

describe('AuthEffects', () => {
  let actions$: Subject<any>;
  let effects: AuthEffects;
  let authService: any;

  beforeEach(() => {
    authService = {
      login: function() {},
      register: function() {},
      logout: function() {}
    };
    actions$ = new Subject();
    TestBed.configureTestingModule({
      providers: [
        AuthEffects,
        provideMockActions(() => actions$),
        provideMockStore(),
        { provide: AuthService, useValue: authService }
      ]
    });
    effects = TestBed.inject(AuthEffects);
  });

  it('should dispatch loginSuccess on login$', () => {
    const credentials = { email: 'a', password: 'b' };
    const token = 'tok';
    const user = { id: 1, fullName: 'T', email: 'a', role: 'Manager' as 'Manager', isActive: true };
    const payload = { nameid: '1', unique_name: 'T', email: 'a', role: 'Manager' };
    (effects as any).parseJwt = () => payload;
    authService.login = () => of({ token });
    let emitted;
    effects.login$.subscribe(result => emitted = result);
    actions$.next(AuthActions.login({ credentials }));
    expect(emitted).toEqual(AuthActions.loginSuccess({ user, token }));
  });

  it('should dispatch loginFailure on login$ error', () => {
    const credentials = { email: 'a', password: 'b' };
    authService.login = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.login$.subscribe(result => emitted = result);
    actions$.next(AuthActions.login({ credentials }));
    expect(emitted).toEqual(AuthActions.loginFailure({ error: 'fail' }));
  });

  it('should dispatch registerSuccess on register$', () => {
    const request = { fullName: 'T', email: 'a', password: 'b', role: 'Employee' };
    authService.register = () => of({ message: 'ok' });
    let emitted;
    effects.register$.subscribe(result => emitted = result);
    actions$.next(AuthActions.register({ request }));
    expect(emitted).toEqual(AuthActions.registerSuccess({ message: 'ok' }));
  });

  it('should dispatch registerFailure on register$ error', () => {
    const request = { fullName: 'T', email: 'a', password: 'b', role: 'Employee' };
    authService.register = () => throwError(() => ({ message: 'fail' }));
    let emitted;
    effects.register$.subscribe(result => emitted = result);
    actions$.next(AuthActions.register({ request }));
    expect(emitted).toEqual(AuthActions.registerFailure({ error: 'fail' }));
  });
});
