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
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import * as AuthActions from '../../../store/actions/auth.actions';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';
import { AppState } from '../../../store';

/**
 * Register component for new user registration
 * Provides form for creating new employee or manager accounts
 */
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;
  hidePassword = true;
  hideConfirmPassword = true;

  roles = [
    { value: 'Employee', label: 'Employee' },
    { value: 'Manager', label: 'Manager' }
  ];

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
   * Initializes the registration form with validators
   */
  private initializeForm(): void {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      role: ['Employee', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  /**
   * Custom validator to check if passwords match
   */
  private passwordMatchValidator(formGroup: FormGroup): { [key: string]: boolean } | null {
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  /**
   * Handles form submission
   */
  onSubmit(): void {
    if (this.registerForm.valid) {
      const { confirmPassword, ...request } = this.registerForm.value;
      this.store.dispatch(AuthActions.register({ request }));
    }
  }

  /**
   * Gets form control for template access
   */
  get fullName() {
    return this.registerForm.get('fullName');
  }

  /**
   * Gets form control for template access
   */
  get email() {
    return this.registerForm.get('email');
  }

  /**
   * Gets form control for template access
   */
  get password() {
    return this.registerForm.get('password');
  }

  /**
   * Gets form control for template access
   */
  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  /**
   * Gets form control for template access
   */
  get role() {
    return this.registerForm.get('role');
  }
}
