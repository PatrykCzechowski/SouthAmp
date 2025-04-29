import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Payment {
  id: number;
  userId: number;
  reservationId: number;
  amount: number;
  status: 'pending' | 'completed' | 'failed' | 'refunded';
  createdAt: string;
  updatedAt: string;
}

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private apiUrl = 'https://api.example.com/payments';

  constructor(private http: HttpClient) {}

  getPayments(): Observable<Payment[]> {
    return this.http.get<Payment[]>(this.apiUrl);
  }

  getPayment(id: number): Observable<Payment> {
    return this.http.get<Payment>(`${this.apiUrl}/${id}`);
  }

  createPayment(data: Partial<Payment>): Observable<Payment> {
    return this.http.post<Payment>(this.apiUrl, data);
  }

  refundPayment(id: number): Observable<Payment> {
    return this.http.post<Payment>(`${this.apiUrl}/${id}/refund`, {});
  }
}
