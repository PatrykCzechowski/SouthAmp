import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Reservation {
  id: number;
  userId: number;
  roomId: number;
  hotelId: number;
  startDate: string;
  endDate: string;
  status: 'active' | 'cancelled' | 'completed';
  totalPrice: number;
}

@Injectable({ providedIn: 'root' })
export class ReservationService {
  private apiUrl = 'https://api.example.com/reservations';

  constructor(private http: HttpClient) {}

  getReservations(): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(this.apiUrl);
  }

  getReservation(id: number): Observable<Reservation> {
    return this.http.get<Reservation>(`${this.apiUrl}/${id}`);
  }

  createReservation(data: Partial<Reservation>): Observable<Reservation> {
    return this.http.post<Reservation>(this.apiUrl, data);
  }

  updateReservation(
    id: number,
    data: Partial<Reservation>
  ): Observable<Reservation> {
    return this.http.put<Reservation>(`${this.apiUrl}/${id}`, data);
  }

  cancelReservation(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, {});
  }
}
