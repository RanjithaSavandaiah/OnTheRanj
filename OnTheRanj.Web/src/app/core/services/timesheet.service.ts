import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Timesheet, TimesheetRequest, TimesheetReviewRequest } from '../models/timesheet.model';

/**
 * Service for managing timesheets
 * Handles employee timesheet submissions and manager approvals
 */
@Injectable({
  providedIn: 'root'
})
export class TimesheetService {
  private readonly API_URL = `${environment.apiUrl}/timesheets`;

  constructor(private http: HttpClient) {}

   /**
   * Submits a draft timesheet (changes status to Submitted)
   */
  submitDraftTimesheet(id: number) {
    return this.http.patch<any>(`${this.API_URL}/${id}/submit`, {});
  }

  /**
   * Gets timesheets for a specific employee
   * @param employeeId Employee ID
   * @param startDate Optional start date filter
   * @param endDate Optional end date filter
   * @returns Observable of timesheet array
   */
  getEmployeeTimesheets(
    employeeId: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<Timesheet[]> {
    let params = new HttpParams();
    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }
    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }

    return this.http.get<Timesheet[]>(`${this.API_URL}/employee/${employeeId}`, { params });
  }

  /**
   * Gets a specific timesheet by ID
   * @param id Timesheet ID
   * @returns Observable of timesheet
   */
  getTimesheetById(id: number): Observable<Timesheet> {
    return this.http.get<Timesheet>(`${this.API_URL}/${id}`);
  }

  /**
   * Submits a new timesheet entry
   * @param request Timesheet entry details
   * @returns Observable of created timesheet
   */
  submitTimesheet(request: TimesheetRequest): Observable<Timesheet> {
    return this.http.post<Timesheet>(this.API_URL, request);
  }

  /**
   * Updates an existing timesheet
   * @param id Timesheet ID
   * @param request Updated timesheet details
   * @returns Observable of updated timesheet
   */
  updateTimesheet(id: number, request: TimesheetRequest): Observable<Timesheet> {
    return this.http.put<Timesheet>(`${this.API_URL}/${id}`, request);
  }

  /**
   * Deletes a timesheet (only if pending)
   * @param id Timesheet ID
   * @returns Observable of void
   */
  deleteTimesheet(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }

  /**
   * Gets all pending timesheets for manager review
   * @returns Observable of pending timesheet array
   */
  getPendingTimesheets(): Observable<Timesheet[]> {
    return this.http.get<Timesheet[]>(`${this.API_URL}/pending`);
  }

  /**
   * Approves or rejects a timesheet
   * @param id Timesheet ID
   * @param review Review decision with optional comments
   * @returns Observable of updated timesheet
   */
  reviewTimesheet(id: number, review: TimesheetReviewRequest): Observable<Timesheet> {
    return this.http.patch<Timesheet>(`${this.API_URL}/${id}/review`, review);
  }
  
  /**
   * Gets all timesheets (for manager dashboard)
   * @returns Observable of all timesheets
   */
  getAllTimesheets(): Observable<Timesheet[]> {
    return this.http.get<Timesheet[]>(`${this.API_URL}`);
  }

  /**
   * Gets timesheets by project
   * @param projectId Project ID
   * @param startDate Optional start date filter
   * @param endDate Optional end date filter
   * @returns Observable of timesheet array
   */
  getTimesheetsByProject(
    projectId: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<Timesheet[]> {
    let params = new HttpParams();
    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }
    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }

    return this.http.get<Timesheet[]>(`${this.API_URL}/project/${projectId}`, { params });
  }
}
