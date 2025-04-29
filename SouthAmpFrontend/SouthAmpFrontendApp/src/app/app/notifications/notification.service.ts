import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface Notification {
  message: string;
  type: 'success' | 'error' | 'info' | 'warning';
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private notificationSubject = new Subject<Notification>();

  get notifications$(): Observable<Notification> {
    return this.notificationSubject.asObservable();
  }

  show(message: string, type: Notification['type'] = 'info') {
    this.notificationSubject.next({ message, type });
  }
}
