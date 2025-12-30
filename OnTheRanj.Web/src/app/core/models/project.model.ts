/**
 * Project code model
 */
export interface ProjectCode {
  id: number;
  code: string;
  projectName: string;
  clientName: string;
  isBillable: boolean;
  status: 'Active' | 'Inactive';
  createdAt: Date;
  updatedAt?: Date;
  createdBy: number;
}

/**
 * Create project request
 */
export interface CreateProjectRequest {
  code: string;
  projectName: string;
  clientName: string;
  isBillable: boolean;
}

/**
 * Update project request
 */
export interface UpdateProjectRequest {
  id: number;
  code: string;
  projectName: string;
  clientName: string;
  isBillable: boolean;
  status: 'Active' | 'Inactive';
}
