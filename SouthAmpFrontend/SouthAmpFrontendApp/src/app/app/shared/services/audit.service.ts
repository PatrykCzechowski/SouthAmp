import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditLog {
  userId?: number;
  action: string;
  entity?: string;
  entityId?: number;
  details?: any;
  timestamp?: string;
}

@Injectable({ providedIn: 'root' })
export class AuditService {
  private apiUrl = 'https://api.example.com/audit';

  constructor(private http: HttpClient) {}

  log(
    action: string,
    entity?: string,
    entityId?: number,
    details?: any
  ): Observable<void> {
    const log: AuditLog = {
      action,
      entity,
      entityId,
      details,
      timestamp: new Date().toISOString(),
    };
    return this.http.post<void>(this.apiUrl, log);
  }
}
