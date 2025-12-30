import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { EmployeeHoursSummary, ProjectHoursSummary, BillableHoursSummary } from '../models/report.model';

/**
 * Service for generating timesheet reports
 * Provides various reporting capabilities for managers
 */
@Injectable({
  providedIn: 'root'
})
export class ReportService {
    /**
     * Gets dashboard summary data from the backend API
     */
    getDashboardSummary(): Observable<any> {
      return this.http.get<any>(`${this.API_URL}/dashboard-summary`);
    }
  private readonly API_URL = `${environment.apiUrl}/reports`;

  constructor(private http: HttpClient) {}

  /**
   * Gets employee hours summary report
   * @param startDate Report start date
   * @param endDate Report end date
   * @returns Observable of employee hours summary array
   */
  getEmployeeHoursSummary(startDate: Date, endDate: Date): Observable<EmployeeHoursSummary[]> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<EmployeeHoursSummary[]>(`${this.API_URL}/employee-hours`, { params });
  }

  /**
   * Gets project hours summary report
   * @param startDate Report start date
   * @param endDate Report end date
   * @returns Observable of project hours summary array
   */
  getProjectHoursSummary(startDate: Date, endDate: Date): Observable<ProjectHoursSummary[]> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<ProjectHoursSummary[]>(`${this.API_URL}/project-hours`, { params });
  }

  /**
   * Gets billable hours summary report
   * @param startDate Report start date
   * @param endDate Report end date
   * @returns Observable of billable hours summary
   */
  getBillableHoursSummary(startDate: Date, endDate: Date): Observable<BillableHoursSummary> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<BillableHoursSummary>(`${this.API_URL}/billable-hours`, { params });
  }

  /**
   * Gets detailed hours breakdown for a specific employee
   * @param employeeId Employee ID
   * @param startDate Report start date
   * @param endDate Report end date
   * @returns Observable of employee hours summary
   */
  getEmployeeDetailReport(
    employeeId: number,
    startDate: Date,
    endDate: Date
  ): Observable<EmployeeHoursSummary> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<EmployeeHoursSummary>(`${this.API_URL}/employee/${employeeId}`, { params });
  }

  /**
   * Exports report to Excel format
   * @param reportType Type of report to export
   * @param startDate Report start date
   * @param endDate Report end date
   * @returns Observable of blob data for download
   */
  exportReport(reportType: 'employee' | 'project' | 'billable', startDate: Date, endDate: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get(`${this.API_URL}/export/${reportType}`, {
      params,
      responseType: 'blob'
    });
  }
}
