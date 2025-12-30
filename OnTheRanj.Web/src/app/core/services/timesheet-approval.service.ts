import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TimesheetApprovalService {
  private baseUrl = '/api/timesheetapprovals';
  private pendingUrl = this.baseUrl + '/pending';

  constructor(private http: HttpClient) {}

  getPendingTimesheets(): Observable<any[]> {
    return this.http.get<any[]>(this.pendingUrl);
  }

  approveTimesheet(id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/approve/${id}`, {});
  }

  getTimesheetsByStatus(status: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/status/${status}`);
  }

  rejectTimesheet(id: number, comments: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/reject/${id}`, { comments });
  }
}
