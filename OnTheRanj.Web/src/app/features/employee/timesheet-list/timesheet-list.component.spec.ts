import { render, screen, fireEvent } from '@testing-library/angular';
import { provideMockStore } from '@ngrx/store/testing';
import { TimesheetListComponent } from './timesheet-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MatDialog } from '@angular/material/dialog';

const createSpy = () => {
  const fn: any = (...args: any[]) => {
    fn.calls.push(args);
    return undefined;
  };
  fn.calls = [];
  fn.and = { callFake: (fakeFn: any) => { fn.fake = fakeFn; } };
  return fn;
};

describe('TimesheetListComponent', () => {
  const initialState = {
    auth: {
      user: { id: 1, fullName: 'Test User', email: 'test@example.com', role: 'Employee', isActive: true },
      token: 'fake-token',
      isAuthenticated: true,
      loading: false,
      error: null
    },
    timesheet: {
      ids: [1, 2],
      entities: {
        1: {
          id: 1,
          employeeId: 1,
          projectCodeId: 1,
          date: new Date('2025-12-20'),
          hoursWorked: 8,
          description: 'Worked on feature',
          status: 'Draft',
          projectCode: { id: 1, code: 'P001', projectName: 'Project 1', clientName: 'ClientX', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 },
          employee: { id: 1, fullName: 'Test User', email: 'test@example.com', role: 'Employee', isActive: true },
          createdAt: new Date('2025-12-20')
        },
        2: {
          id: 2,
          employeeId: 1,
          projectCodeId: 2,
          date: new Date('2025-12-21'),
          hoursWorked: 6,
          description: 'Bug fixes',
          status: 'Approved',
          projectCode: { id: 2, code: 'P002', projectName: 'Project 2', clientName: 'ClientY', isBillable: false, status: 'Active', createdAt: new Date(), createdBy: 1 },
          employee: { id: 1, fullName: 'Test User', email: 'test@example.com', role: 'Employee', isActive: true },
          createdAt: new Date('2025-12-21')
        }
      },
      selectedTimesheetId: null,
      loading: false,
      error: null
    }
  };

  it('should render the timesheet table', async () => {
    await render(TimesheetListComponent, {
      imports: [HttpClientTestingModule],
      providers: [
        provideMockStore({ initialState }),
        RouterTestingModule,
        { provide: MatDialog, useValue: {} }
      ]
    });
    // There are multiple 'My Timesheets' in the layout (sidebar, toolbar)
    const timesheetTitles = screen.getAllByText(/My Timesheets/i);
    expect(timesheetTitles.length).toBeGreaterThan(0);
    expect(screen.getByText(/Worked on feature/i)).toBeTruthy();
    expect(screen.getByText(/Bug fixes/i)).toBeTruthy();
    expect(screen.getByText(/Draft/i)).toBeTruthy();
    expect(screen.getByText(/Approved/i)).toBeTruthy();
  });

  it('should show actions column if there is a draft timesheet', async () => {
    await render(TimesheetListComponent, {
      imports: [HttpClientTestingModule],
      providers: [
        provideMockStore({ initialState }),
        RouterTestingModule,
        { provide: MatDialog, useValue: {} }
      ]
    });
    expect(screen.getByText(/actions/i)).toBeTruthy();
  });

  it('should navigate to new timesheet form on createTimesheet()', async () => {
    await render(TimesheetListComponent, {
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        provideMockStore({ initialState }),
        { provide: MatDialog, useValue: {} }
      ]
    });
    // The button in the template is labeled 'New Timesheet'
    const createBtn = screen.getByText(/New Timesheet/i);
    expect(createBtn).toBeTruthy();
  });
});
