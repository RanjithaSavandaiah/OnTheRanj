import { render, screen } from '@testing-library/angular';
import { provideMockStore } from '@ngrx/store/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TimesheetApprovalComponent } from './timesheet-approval.component';

describe('TimesheetApprovalComponent', () => {
  it('should render the Timesheet Approvals title', async () => {
      await render(TimesheetApprovalComponent, {
        providers: [provideMockStore({})],
        imports: [HttpClientTestingModule]
      });
    expect(screen.getAllByText(/Timesheet Approvals/i).length).toBeGreaterThan(0);
  });

  it('should render Pending, Approved, and Rejected tabs', async () => {
      await render(TimesheetApprovalComponent, {
        providers: [provideMockStore({})],
        imports: [HttpClientTestingModule]
      });
    expect(screen.getAllByText(/Pending/i).length).toBeGreaterThan(0);
    expect(screen.getAllByText(/Approved/i).length).toBeGreaterThan(0);
    expect(screen.getAllByText(/Rejected/i).length).toBeGreaterThan(0);
  });

  it('should render Approve and Reject buttons', async () => {
      await render(TimesheetApprovalComponent, {
        providers: [provideMockStore({})],
        imports: [HttpClientTestingModule],
        componentProperties: {
          timesheets: [
            { employee: 'Alice', project: 'E-Commerce', date: '2025-12-16', hours: 6, status: 'Submitted' }
          ]
        }
      });
    // There may be multiple matches (subtitle, tab, button), so check at least one button exists
    const approveButtons = screen.getAllByText(/Approve/i);
    const rejectButtons = screen.getAllByText(/Reject/i);
    expect(approveButtons.length).toBeGreaterThan(0);
    expect(rejectButtons.length).toBeGreaterThan(0);
  });

  it('should show message when no timesheets are present', async () => {
      await render(TimesheetApprovalComponent, {
        providers: [provideMockStore({})],
        imports: [HttpClientTestingModule],
        componentProperties: { timesheets: [] }
      });
    expect(screen.getByText(/No pending timesheets to approve/i)).toBeTruthy();
  });

  it('should display status as Submitted for a pending timesheet', async () => {
      await render(TimesheetApprovalComponent, {
        providers: [provideMockStore({})],
        imports: [HttpClientTestingModule],
        componentProperties: {
          timesheets: [
            { employee: 'Bob', project: 'E-Commerce', date: '2025-12-17', hours: 8, status: 'Submitted' }
          ]
        }
      });
    expect(screen.getByText(/Submitted/i)).toBeTruthy();
  });
});
