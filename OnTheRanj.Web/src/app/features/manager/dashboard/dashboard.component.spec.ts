import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { ProjectService } from '../../../core/services/project.service';
import { UserService } from '../../../core/services/user.service';
import { TimesheetService } from '../../../core/services/timesheet.service';
import { logout } from '../../../store/actions/auth.actions';

const mockProjects = [{}, {}, {}];
const mockUsers = [
  { id: 1, role: 'Employee' },
  { id: 2, role: 'Employee' },
  { id: 3, role: 'Manager' }
];
const mockTimesheets = [{}, {}];
const mockPending = [{}];

const initialState = {
  auth: {
    user: { id: 1, fullName: 'Manager', email: 'manager@example.com', role: 'Manager' },
    loading: false,
    error: null
  }
};

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let store: MockStore;
  let routerNavigateSpy: { calls: any[]; fn: Function };

  beforeEach(async () => {
    routerNavigateSpy = { calls: [], fn: function() { routerNavigateSpy.calls.push(Array.from(arguments)); } };
    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        provideMockStore({ initialState }),
        { provide: Router, useValue: { navigate: (...args: any[]) => routerNavigateSpy.fn(...args) } },
        { provide: ProjectService, useValue: { getAllProjects: () => of(mockProjects) } },
        { provide: UserService, useValue: { getAll: () => of(mockUsers) } },
        { provide: TimesheetService, useValue: {
          getAllTimesheets: () => of(mockTimesheets),
          getPendingTimesheets: () => of(mockPending)
        } },
        { provide: ActivatedRoute, useValue: {} }
      ]
    }).compileComponents();
    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load project, employee, timesheet, and pending approval counts on init', () => {
    expect(component.projectCount).toBe(3);
    expect(component.employeeCount).toBe(2);
    expect(component.timesheetCount).toBe(2);
    expect(component.pendingApprovalCount).toBe(1);
  });

  it('should dispatch logout and navigate to login on onLogout()', () => {
    // Manual spy for dispatch
    const dispatchSpy: { calls: any[][] } = { calls: [] };
    const origDispatch = store.dispatch.bind(store);
    store.dispatch = ((action: any) => { dispatchSpy.calls.push([action]); return origDispatch(action); }) as any;
    component.onLogout();
    expect(dispatchSpy.calls.some((call: any[]) => call[0] && call[0].type === logout.type)).toBe(true);
    expect(routerNavigateSpy.calls.some(call => JSON.stringify(call[0]) === JSON.stringify(['/login']))).toBe(true);
  });
});
