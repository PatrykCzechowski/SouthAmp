import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Review {
  id: number;
  userId: number;
  hotelId: number;
  reservationId: number;
  rating: number;
  comment: string;
  createdAt: string;
  updatedAt: string;
}

@Injectable({ providedIn: 'root' })
export class ReviewService {
  private apiUrl = 'https://api.example.com/reviews';

  constructor(private http: HttpClient) {}

  getReviews(hotelId?: number): Observable<Review[]> {
    const url = hotelId ? `${this.apiUrl}?hotelId=${hotelId}` : this.apiUrl;
    return this.http.get<Review[]>(url);
  }

  getReview(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/${id}`);
  }

  createReview(data: Partial<Review>): Observable<Review> {
    return this.http.post<Review>(this.apiUrl, data);
  }

  updateReview(id: number, data: Partial<Review>): Observable<Review> {
    return this.http.put<Review>(`${this.apiUrl}/${id}`, data);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
