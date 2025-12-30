import { authInterceptor } from './auth.interceptor';
import { AuthService } from '../services/auth.service';
import { HttpRequest } from '@angular/common/http';
import { runInInjectionContext, createEnvironmentInjector, EnvironmentInjector } from '@angular/core';
import { TestBed } from '@angular/core/testing';

describe('authInterceptor', () => {
  let tokenValue: string | null;
  let nextCalledWith: any[] | null;
  let authService: any;
  let next: any;
  let injector: EnvironmentInjector;

  beforeEach(() => {
    tokenValue = null;
    nextCalledWith = null;
    authService = { getToken: () => tokenValue };
    next = (req: any) => { nextCalledWith = [req]; };
    injector = createEnvironmentInjector([
      { provide: AuthService, useValue: authService }
    ], TestBed.inject(EnvironmentInjector));
  });

  function runInterceptor(req: any, nextFn: any) {
    return runInInjectionContext(injector, () => authInterceptor(req, nextFn));
  }

  it('should add Authorization header if token exists', () => {
    tokenValue = 'abc123';
    const req = new HttpRequest('GET', '/api/test');
    runInterceptor(req, next);
    expect(nextCalledWith).not.toBeNull();
    const calledReq = nextCalledWith![0];
    expect(calledReq.headers.get('Authorization')).toBe('Bearer abc123');
  });

  it('should not add Authorization header if no token', () => {
    tokenValue = null;
    const req = new HttpRequest('GET', '/api/test');
    runInterceptor(req, next);
    expect(nextCalledWith![0]).toBe(req);
  });
});

