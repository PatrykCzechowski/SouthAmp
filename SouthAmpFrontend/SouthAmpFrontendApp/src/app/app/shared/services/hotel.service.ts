import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Hotel {
  id: number;
  name: string;
  location: string;
  description: string;
  imageUrl?: string;
  rating?: number;
  city?: string;
  country?: string;
  approved?: boolean;
}

@Injectable({ providedIn: 'root' })
export class HotelService {
  private apiUrl = 'https://api.example.com/hotels';

  constructor(private http: HttpClient) {}

  getHotels(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(this.apiUrl);
  }

  getHotel(id: number): Observable<Hotel> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  createHotel(data: Partial<Hotel>) {
    return this.http.post<Hotel>(this.apiUrl, data);
  }

  updateHotel(id: number, data: Partial<Hotel>) {
    return this.http.put<Hotel>(`${this.apiUrl}/${id}`, data);
  }

  // Dalsze metody: deleteHotel, searchHotels, itp.
}
