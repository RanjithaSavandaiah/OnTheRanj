import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import * as AuthActions from '../../../store/actions/auth.actions';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';
import { AppState } from '../../../store';

/**
 * Login component for user authentication
 * Provides email/password login with form validation
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private store: Store<AppState>,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loading$ = this.store.select(AuthSelectors.selectAuthLoading);
    this.error$ = this.store.select(AuthSelectors.selectAuthError);
    this.initializeForm();
  }

  /**
   * Initializes the login form with validators
   */
  private initializeForm(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  /**
   * Handles form submission
   */
  onSubmit(): void {
    if (this.loginForm.valid) {
      this.store.dispatch(
        AuthActions.login({ credentials: this.loginForm.value })
      );
    }
  }

  /**
   * Gets form control for template access
   */
  get email() {
    return this.loginForm.get('email');
  }

  /**
   * Gets form control for template access
   */
  get password() {
    return this.loginForm.get('password');
  }
}
