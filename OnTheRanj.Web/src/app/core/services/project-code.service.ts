import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProjectCode } from '../models/project.model';

@Injectable({ providedIn: 'root' })
export class ProjectCodeService {
  private readonly apiUrl = '/api/Projects';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ProjectCode[]> {
    return this.http.get<ProjectCode[]>(this.apiUrl);
  }

  getActive(): Observable<ProjectCode[]> {
    return this.http.get<ProjectCode[]>(`${this.apiUrl}/active`);
  }
}
