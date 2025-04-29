import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Report {
  id: number;
  userId: number;
  type: string;
  description: string;
  status: 'open' | 'in_progress' | 'closed';
  createdAt: string;
  updatedAt: string;
  adminResponse?: string;
}

@Injectable({ providedIn: 'root' })
export class ReportService {
  private apiUrl = 'https://api.example.com/reports';

  constructor(private http: HttpClient) {}

  getReports(): Observable<Report[]> {
    return this.http.get<Report[]>(this.apiUrl);
  }

  getReport(id: number): Observable<Report> {
    return this.http.get<Report>(`${this.apiUrl}/${id}`);
  }

  createReport(data: Partial<Report>): Observable<Report> {
    return this.http.post<Report>(this.apiUrl, data);
  }

  updateReport(id: number, data: Partial<Report>): Observable<Report> {
    return this.http.put<Report>(`${this.apiUrl}/${id}`, data);
  }
}
