import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EmailNotificationService {
  private apiUrl = 'https://api.example.com/notifications/email';

  constructor(private http: HttpClient) {}

  sendEmail(to: string, subject: string, message: string): Observable<void> {
    return this.http.post<void>(this.apiUrl, { to, subject, message });
  }
}
