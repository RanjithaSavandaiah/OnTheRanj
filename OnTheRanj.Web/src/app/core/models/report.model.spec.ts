import { EmployeeHoursSummary, ProjectHoursSummary, BillableHoursSummary } from './report.model';

describe('EmployeeHoursSummary Model', () => {
  it('should create a valid EmployeeHoursSummary object', () => {
    const summary: EmployeeHoursSummary = {
      employeeId: 1,
      employeeName: 'Alice',
      totalHours: 40,
      billableHours: 32,
      nonBillableHours: 8
    };
    expect(summary.employeeId).toBe(1);
    expect(summary.employeeName).toBe('Alice');
    expect(summary.totalHours).toBe(40);
    expect(summary.billableHours).toBe(32);
    expect(summary.nonBillableHours).toBe(8);
  });
});

describe('ProjectHoursSummary Model', () => {
  it('should create a valid ProjectHoursSummary object', () => {
    const summary: ProjectHoursSummary = {
      projectCodeId: 2,
      projectCode: 'P123',
      projectName: 'Project X',
      clientName: 'Client Y',
      totalHours: 100,
      employeeCount: 5
    };
    expect(summary.projectCodeId).toBe(2);
    expect(summary.projectCode).toBe('P123');
    expect(summary.projectName).toBe('Project X');
    expect(summary.clientName).toBe('Client Y');
    expect(summary.totalHours).toBe(100);
    expect(summary.employeeCount).toBe(5);
  });
});

describe('BillableHoursSummary Model', () => {
  it('should create a valid BillableHoursSummary object', () => {
    const summary: BillableHoursSummary = {
      totalBillableHours: 80,
      totalNonBillableHours: 20,
      totalHours: 100,
      billablePercentage: 80
    };
    expect(summary.totalBillableHours).toBe(80);
    expect(summary.totalNonBillableHours).toBe(20);
    expect(summary.totalHours).toBe(100);
    expect(summary.billablePercentage).toBe(80);
  });
});
