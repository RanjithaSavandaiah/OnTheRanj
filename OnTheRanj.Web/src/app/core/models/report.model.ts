/**
 * Report models for manager reports
 */

export interface EmployeeHoursSummary {
  employeeId: number;
  employeeName: string;
  totalHours: number;
  billableHours: number;
  nonBillableHours: number;
}

export interface ProjectHoursSummary {
  projectCodeId: number;
  projectCode: string;
  projectName: string;
  clientName: string;
  totalHours: number;
  employeeCount: number;
}

export interface BillableHoursSummary {
  totalBillableHours: number;
  totalNonBillableHours: number;
  totalHours: number;
  billablePercentage: number;
}
