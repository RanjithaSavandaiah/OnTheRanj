import { authGuard } from './auth.guard';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { runInInjectionContext, createEnvironmentInjector, EnvironmentInjector } from '@angular/core';
import { TestBed } from '@angular/core/testing';

describe('authGuard', () => {
  let isAuthenticatedValue: boolean;
  let navigateCalledWith: any[] | null;
  let authService: any;
  let router: any;
  let state: any;
  let injector: EnvironmentInjector;

  beforeEach(() => {
    isAuthenticatedValue = false;
    navigateCalledWith = null;
    authService = { isAuthenticated: () => isAuthenticatedValue };
    router = { navigate: (...args: any[]) => { navigateCalledWith = args; } };
    state = { url: '/protected' };
    injector = createEnvironmentInjector([
      { provide: AuthService, useValue: authService },
      { provide: Router, useValue: router }
    ], TestBed.inject(EnvironmentInjector));
  });

  function runGuard(route: any, state: any) {
    return runInInjectionContext(injector, () => authGuard(route, state));
  }

  it('should allow access if authenticated', () => {
    isAuthenticatedValue = true;
    expect(runGuard({} as any, state)).toBe(true);
    expect(navigateCalledWith).toBeNull();
  });

  it('should redirect to login if not authenticated', () => {
    isAuthenticatedValue = false;
    expect(runGuard({} as any, state)).toBe(false);
    expect(navigateCalledWith).toEqual([
      ['/login'], { queryParams: { returnUrl: state.url } }
    ]);
  });
});
