import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { Router } from '@angular/router';
import * as AuthActions from '../../../store/actions/auth.actions';

const initialState = {
  auth: {
    loading: false,
    error: null
  }
};

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let store: MockStore;
  let dispatchSpy: { calls: any[] };
  let routerNavigateSpy: { calls: any[]; fn: Function };

  beforeEach(async () => {
    // Manual spy for router.navigate
    routerNavigateSpy = { calls: [], fn: function() { routerNavigateSpy.calls.push(Array.from(arguments)); } };
    await TestBed.configureTestingModule({
      imports: [RegisterComponent, ReactiveFormsModule],
      providers: [
        provideMockStore({ initialState }),
        { provide: Router, useValue: {
          navigate: (...args: any[]) => routerNavigateSpy.fn(...args),
          createUrlTree: () => ({}),
          serializeUrl: () => '',
          events: { subscribe: () => ({ unsubscribe: () => {} }) }
        } },
        { provide: ActivatedRoute, useValue: {} }
      ]
    }).compileComponents();
    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    // Manual spy for dispatch
    dispatchSpy = { calls: [] };
    const origDispatch = store.dispatch.bind(store);
    store.dispatch = ((action: any) => { dispatchSpy.calls.push([action]); return origDispatch(action); }) as any;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    expect(component.registerForm).toBeTruthy();
    expect(component.registerForm.get('role')?.value).toBe('Employee');
  });

  it('should mark form as invalid if required fields are missing', () => {
    component.registerForm.patchValue({
      fullName: '',
      email: '',
      password: '',
      confirmPassword: '',
      role: ''
    });
    expect(component.registerForm.invalid).toBe(true);
  });

  it('should set passwordMismatch error if passwords do not match', () => {
    component.registerForm.patchValue({
      password: 'password1',
      confirmPassword: 'password2'
    });
    expect(component.registerForm.errors).toEqual({ passwordMismatch: true });
  });

  it('should dispatch register action on valid form submit', () => {
    component.registerForm.patchValue({
      fullName: 'Test User',
      email: 'test@example.com',
      password: 'password',
      confirmPassword: 'password',
      role: 'Employee'
    });
    component.onSubmit();
    // Check that dispatch was called with an action of type register
    const found = dispatchSpy.calls.some((call: any) => call[0] && call[0].type === AuthActions.register.type);
    expect(found).toBe(true);
  });

  it('should not dispatch register action if form is invalid', () => {
    component.registerForm.patchValue({
      fullName: '',
      email: '',
      password: '',
      confirmPassword: '',
      role: ''
    });
    component.onSubmit();
    expect(dispatchSpy.calls.length).toBe(0);
  });
});
