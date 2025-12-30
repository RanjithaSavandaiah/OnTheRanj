import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProjectAssignment {
  id: number;
  employeeId: number;
  projectCodeId: number;
  projectName?: string;
  startDate: string;
  endDate?: string | null;
  createdAt?: string;
  createdBy?: number;
}

export interface CreateAssignmentRequest {
  employeeId: number;
  projectCodeId: number;
  startDate: string;
  endDate?: string;
}

@Injectable({ providedIn: 'root' })
export class ProjectAssignmentService {
  private readonly apiUrl = '/api/ProjectAssignments';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ProjectAssignment[]> {
    return this.http.get<ProjectAssignment[]>(`${this.apiUrl}`);
  }

  getAllByEmployee(employeeId: number): Observable<ProjectAssignment[]> {
    return this.http.get<ProjectAssignment[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  getAllByProject(projectCodeId: number): Observable<ProjectAssignment[]> {
    return this.http.get<ProjectAssignment[]>(`${this.apiUrl}/project/${projectCodeId}`);
  }

  getById(id: number): Observable<ProjectAssignment> {
    return this.http.get<ProjectAssignment>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateAssignmentRequest): Observable<ProjectAssignment> {
    return this.http.post<ProjectAssignment>(this.apiUrl, request);
  }

  update(id: number, request: Partial<ProjectAssignment>): Observable<ProjectAssignment> {
    return this.http.put<ProjectAssignment>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getActiveAssignments(employeeId: number): Observable<ProjectAssignment[]> {
    return this.http.get<ProjectAssignment[]>(`${this.apiUrl}/employee/${employeeId}/active`);
  }
}
