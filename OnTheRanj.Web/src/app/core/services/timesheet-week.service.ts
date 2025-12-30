import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TimesheetWeekEntry {
  projectCodeId: number;
  date: string; // ISO string
  hoursWorked: number;
  description: string;
}

export interface TimesheetWeekSubmitRequest {
  employeeId: number;
  entries: TimesheetWeekEntry[];
}

@Injectable({ providedIn: 'root' })
export class TimesheetWeekService {
  private apiUrl = '/api/timesheets/week';

  constructor(private http: HttpClient) {}

  submitWeek(request: TimesheetWeekSubmitRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }
}
