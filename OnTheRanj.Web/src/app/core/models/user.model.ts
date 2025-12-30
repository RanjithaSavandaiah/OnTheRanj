/**
 * User model representing authenticated user
 */
export interface User {
  id: number;
  fullName: string;
  email: string;
  role: 'Employee' | 'Manager';
  isActive: boolean;
}

/**
 * Login request payload
 */
export interface LoginRequest {
  email: string;
  password: string;
}

/**
 * Login response from API
 */
export interface LoginResponse {
  token: string;
  message: string;
}

/**
 * Register request payload
 */
export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: string;
}

/**
 * Register response from API
 */
export interface RegisterResponse {
  userId: number;
  fullName: string;
  email: string;
  role: string;
  message: string;
}
