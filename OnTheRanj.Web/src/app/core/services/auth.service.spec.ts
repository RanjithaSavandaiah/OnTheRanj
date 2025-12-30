import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../models/user.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let navigateCalls: any[];
  let routerSpy: { navigate: (...args: any[]) => void };

  beforeEach(() => {
    navigateCalls = [];
    routerSpy = { navigate: (...args: any[]) => { navigateCalls.push(args); } };
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: Router, useValue: routerSpy }
      ]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and set token', () => {
    const credentials: LoginRequest = { email: 'test@example.com', password: 'pass' };
    const response: LoginResponse = {
      token: 'header.' + btoa(JSON.stringify({
        nameid: '1', unique_name: 'Test', email: 'test@example.com', role: 'Manager' })) + '.sig',
      message: 'Login successful'
    };
    service.login(credentials).subscribe(res => {
      expect(res).toEqual(response);
      expect(localStorage.getItem('token')).toBe(response.token);
      expect(service.isAuthenticated()).toBe(true);
      expect(service.currentUser()?.email).toBe('test@example.com');
    });
    const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
    expect(req.request.method).toBe('POST');
    req.flush(response);
  });

  it('should register', () => {
    const request: RegisterRequest = { email: 'new@example.com', password: 'pass', fullName: 'New', role: 'Employee' };
    const response: RegisterResponse = { userId: 1, message: 'Registered', fullName: 'New', email: 'new@example.com', role: 'Employee' };
    service.register(request).subscribe(res => {
      expect(res).toEqual(response);
    });
    const req = httpMock.expectOne(`${environment.apiUrl}/auth/register`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(response);
  });

  it('should logout and clear state', () => {
    localStorage.setItem('token', 'test');
    service.currentUser.set({ id: 1, fullName: 'Test', email: 'test@example.com', role: 'Manager', isActive: true });
    service.isAuthenticated.set(true);
    service.logout();
    expect(localStorage.getItem('token')).toBeNull();
    expect(service.currentUser()).toBeNull();
    expect(service.isAuthenticated()).toBe(false);
    expect(navigateCalls.length).toBe(1);
    expect(navigateCalls[0][0]).toEqual(['/login']);
  });

  it('should get token from localStorage', () => {
    localStorage.setItem('token', 'abc');
    expect(service.getToken()).toBe('abc');
  });

  it('should return false for hasRole if not logged in', () => {
    expect(service.hasRole('Manager')).toBe(false);
  });

  it('should return true for hasRole if user has role', () => {
    service.currentUser.set({ id: 1, fullName: 'Test', email: 'test@example.com', role: 'Manager', isActive: true });
    expect(service.hasRole('Manager')).toBe(true);
  });
});
