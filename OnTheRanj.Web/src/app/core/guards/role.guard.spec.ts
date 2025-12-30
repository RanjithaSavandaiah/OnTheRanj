import { roleGuard } from './role.guard';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { runInInjectionContext, createEnvironmentInjector, EnvironmentInjector } from '@angular/core';
import { TestBed } from '@angular/core/testing';

describe('roleGuard', () => {
  let hasRoleValue: boolean;
  let navigateCalledWith: any[] | null;
  let authService: any;
  let router: any;
  let route: any;
  let state: any;
  let injector: EnvironmentInjector;

  beforeEach(() => {
    hasRoleValue = false;
    navigateCalledWith = null;
    authService = { hasRole: () => hasRoleValue };
    router = { navigate: (...args: any[]) => { navigateCalledWith = args; } };
    route = { data: { role: 'Manager' } };
    state = { url: '/manager' };
    injector = createEnvironmentInjector([
      { provide: AuthService, useValue: authService },
      { provide: Router, useValue: router }
    ], TestBed.inject(EnvironmentInjector));
  });

  function runGuard(route: any, state: any) {
    return runInInjectionContext(injector, () => roleGuard(route, state));
  }

  it('should allow access if user has required role', () => {
    hasRoleValue = true;
    expect(runGuard(route, state)).toBe(true);
    expect(navigateCalledWith).toBeNull();
  });

  it('should redirect to unauthorized if user lacks role', () => {
    hasRoleValue = false;
    expect(runGuard(route, state)).toBe(false);
    expect(navigateCalledWith).toEqual([
      ['/unauthorized']
    ]);
  });
});

