import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Authentication guard for protecting routes
 * Redirects unauthenticated users to login page
 */
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  // Store the attempted URL for redirecting after login
  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
