import { ProjectCode } from './project.model';
import { User } from './user.model';

/**
 * Timesheet status enum
 */
export type TimesheetStatus = 'Draft' | 'Submitted' | 'Approved' | 'Rejected';

/**
 * Timesheet model
 */
export interface Timesheet {
  id: number;
  employeeId: number;
  projectCodeId: number;
  date: Date;
  hoursWorked: number;
  description: string;
  status: TimesheetStatus;
  managerComments?: string;
  reviewedBy?: number;
  reviewedAt?: Date;
  createdAt: Date;
  updatedAt?: Date;
  employee?: User;
  projectCode?: ProjectCode;
  reviewer?: User;
}

/**
 * Create/Update timesheet request
 */
export interface TimesheetRequest {
  employeeId: number;
  projectCodeId: number;
  date: string;
  hoursWorked: number;
  description: string;
}

/**
 * Approve/Reject timesheet request
 */
export interface TimesheetReviewRequest {
  comments?: string;
}
