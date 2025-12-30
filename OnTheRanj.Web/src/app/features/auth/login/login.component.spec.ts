import { ComponentFixture, TestBed } from '@angular/core/testing';
declare const jasmine: any;
import { LoginComponent } from './login.component';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { By } from '@angular/platform-browser';
import * as AuthActions from '../../../store/actions/auth.actions';
import * as AuthSelectors from '../../../store/selectors/auth.selectors';
import { of } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        LoginComponent,
        ReactiveFormsModule,
        RouterTestingModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatIconModule
      ],
      providers: [
        provideMockStore({
          selectors: [
            { selector: AuthSelectors.selectAuthLoading, value: of(false) },
            { selector: AuthSelectors.selectAuthError, value: of(null) }
          ]
        })
      ]
    }).compileComponents();
    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('should validate email and password fields', () => {
    const email = component.loginForm.get('email');
    const password = component.loginForm.get('password');
    email?.setValue('not-an-email');
    password?.setValue('123');
    expect(email?.valid).toBeFalsy();
    expect(password?.valid).toBeFalsy();
    email?.setValue('test@example.com');
    password?.setValue('123456');
    expect(email?.valid).toBeTruthy();
    expect(password?.valid).toBeTruthy();
  });

  it('should dispatch login action on valid submit', () => {
    let calledWith: any = null;
    (store as any).dispatch = (action: any) => { calledWith = action; return action; };
    component.loginForm.setValue({ email: 'test@example.com', password: '123456' });
    component.onSubmit();
    expect(calledWith).toEqual(
      AuthActions.login({ credentials: { email: 'test@example.com', password: '123456' } })
    );
  });

  it('should not dispatch login action on invalid submit', () => {
    let called = false;
    (store as any).dispatch = () => { called = true; };
    component.loginForm.setValue({ email: '', password: '' });
    component.onSubmit();
    expect(called).toBeFalsy();
  });

  it('should toggle hidePassword', () => {
    expect(component.hidePassword).toBeTruthy();
    component.hidePassword = false;
    expect(component.hidePassword).toBeFalsy();
  });
});
