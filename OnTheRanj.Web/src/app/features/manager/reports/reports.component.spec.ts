import { render, screen } from '@testing-library/angular';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReportsComponent } from './reports.component';

describe('ReportsComponent', () => {
  it('should render the Reports title', async () => {
      await render(ReportsComponent, {
        imports: [HttpClientTestingModule]
      });
    expect(screen.getAllByText(/Reports/i).length).toBeGreaterThan(0);
  });

  it('should render Employee Summary, Project Summary, and Billable tabs', async () => {
      await render(ReportsComponent, {
        imports: [HttpClientTestingModule]
      });
    expect(screen.getByText(/Employee Summary/i)).toBeTruthy();
    expect(screen.getByText(/Project Summary/i)).toBeTruthy();
    expect(screen.getByText(/Billable/i)).toBeTruthy();
  });

  it('should render Start Date and End Date fields', async () => {
      await render(ReportsComponent, {
        imports: [HttpClientTestingModule]
      });
    expect(screen.getByLabelText(/Start Date/i)).toBeTruthy();
    expect(screen.getByLabelText(/End Date/i)).toBeTruthy();
  });

  it('should render Excel and PDF export buttons', async () => {
      await render(ReportsComponent, {
        imports: [HttpClientTestingModule]
      });
    expect(screen.getAllByText(/Excel/i).length).toBeGreaterThan(0);
    expect(screen.getAllByText(/PDF/i).length).toBeGreaterThan(0);
  });
});
