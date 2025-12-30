import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../models/user.model';

/**
 * Authentication service handling user login, registration, and token management
 * Uses Angular Signals for reactive state management
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = `${environment.apiUrl}/auth`;
  
  /** Angular Signal for current authenticated user */
  currentUser = signal<User | null>(null);
  
  /** Angular Signal for authentication status */
  isAuthenticated = signal<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadUserFromToken();
  }

  /**
   * Authenticates user with email and password
   * @param credentials Login credentials
   * @returns Observable of login response with JWT token
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_URL}/login`, credentials)
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          this.loadUserFromToken();
        })
      );
  }

  /**
   * Registers a new user
   * @param request Registration details
   * @returns Observable of registration response
   */
  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.API_URL}/register`, request);
  }

  /**
   * Logs out current user and clears authentication state
   */
  logout(): void {
    localStorage.removeItem('token');
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }

  /**
   * Gets JWT token from local storage
   * @returns JWT token or null
   */
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  /**
   * Loads user information from JWT token stored in local storage
   */
  private loadUserFromToken(): void {
    const token = this.getToken();
    if (token) {
      const payload = this.parseJwt(token);
      if (payload) {
        this.currentUser.set({
          id: parseInt(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.nameid),
          fullName: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || payload.unique_name,
          email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || payload.email,
          role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role,
          isActive: true
        });
        this.isAuthenticated.set(true);
      }
    }
  }

  /**
   * Parses JWT token to extract payload
   * @param token JWT token string
   * @returns Decoded token payload or null
   */
  private parseJwt(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }

  /**
   * Checks if current user has specific role
   * @param role Role to check
   * @returns True if user has the role
   */
  hasRole(role: string): boolean {
    return this.currentUser()?.role === role;
  }
}
