import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProjectCode, CreateProjectRequest, UpdateProjectRequest } from '../models/project.model';

/**
 * Service for managing project codes
 * Provides CRUD operations for projects
 */
@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly API_URL = `${environment.apiUrl}/projects`;

  constructor(private http: HttpClient) {}

  /**
   * Gets all active project codes
   * @returns Observable of project code array
   */
  getAllProjects(): Observable<ProjectCode[]> {
    return this.http.get<ProjectCode[]>(this.API_URL);
  }

  /**
   * Gets a specific project by ID
   * @param id Project ID
   * @returns Observable of project code
   */
  getProjectById(id: number): Observable<ProjectCode> {
    return this.http.get<ProjectCode>(`${this.API_URL}/${id}`);
  }

  /**
   * Creates a new project code
   * @param request Project creation details
   * @returns Observable of created project
   */
  createProject(request: CreateProjectRequest): Observable<ProjectCode> {
    return this.http.post<ProjectCode>(this.API_URL, request);
  }

  /**
   * Updates an existing project
   * @param id Project ID
   * @param request Updated project details
   * @returns Observable of updated project
   */
  updateProject(id: number, request: UpdateProjectRequest): Observable<ProjectCode> {
    return this.http.put<ProjectCode>(`${this.API_URL}/${id}`, request);
  }

  /**
   * Deactivates a project (soft delete)
   * @param id Project ID
   * @returns Observable of void
   */
  deactivateProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }

  /**
   * Gets billable projects only
   * @returns Observable of billable project codes
   */
  getBillableProjects(): Observable<ProjectCode[]> {
    return this.http.get<ProjectCode[]>(`${this.API_URL}/billable`);
  }
}
