import { ProjectCode } from './project.model';
import { User } from './user.model';

/**
 * Project assignment model
 */
export interface ProjectAssignment {
  id: number;
  employeeId: number;
  projectCodeId: number;
  startDate: Date;
  endDate?: Date;
  createdAt: Date;
  createdBy: number;
  employee?: User;
  projectCode?: ProjectCode;
}

/**
 * Create assignment request
 */
export interface CreateAssignmentRequest {
  employeeId: number;
  projectCodeId: number;
  startDate: string;
  endDate?: string;
}
