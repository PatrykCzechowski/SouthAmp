import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Discount {
  id: number;
  code: string;
  percent: number;
  validFrom: string;
  validTo: string;
  usageLimit: number;
  usedCount: number;
  active: boolean;
}

@Injectable({ providedIn: 'root' })
export class DiscountService {
  private apiUrl = 'https://api.example.com/discounts';

  constructor(private http: HttpClient) {}

  getDiscounts(): Observable<Discount[]> {
    return this.http.get<Discount[]>(this.apiUrl);
  }

  getDiscount(id: number): Observable<Discount> {
    return this.http.get<Discount>(`${this.apiUrl}/${id}`);
  }

  createDiscount(data: Partial<Discount>): Observable<Discount> {
    return this.http.post<Discount>(this.apiUrl, data);
  }

  updateDiscount(id: number, data: Partial<Discount>): Observable<Discount> {
    return this.http.put<Discount>(`${this.apiUrl}/${id}`, data);
  }

  validateCode(code: string): Observable<Discount> {
    return this.http.get<Discount>(`${this.apiUrl}/validate/${code}`);
  }
}
