import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Role-based authorization guard
 * Checks if user has required role to access route
 */
export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const requiredRole = route.data['role'] as string;
  
  if (authService.hasRole(requiredRole)) {
    return true;
  }

  // Redirect to unauthorized page or dashboard
  router.navigate(['/unauthorized']);
  return false;
};
