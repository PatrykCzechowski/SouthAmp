import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Room {
  id: number;
  hotelId: number;
  name: string;
  description: string;
  price: number;
  available: boolean;
  images?: string[];
}

@Injectable({ providedIn: 'root' })
export class RoomService {
  private apiUrl = 'https://api.example.com/rooms';

  constructor(private http: HttpClient) {}

  getRooms(hotelId?: number): Observable<Room[]> {
    const url = hotelId ? `${this.apiUrl}?hotelId=${hotelId}` : this.apiUrl;
    return this.http.get<Room[]>(url);
  }

  getRoom(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.apiUrl}/${id}`);
  }

  createRoom(data: Partial<Room>): Observable<Room> {
    return this.http.post<Room>(this.apiUrl, data);
  }

  updateRoom(id: number, data: Partial<Room>): Observable<Room> {
    return this.http.put<Room>(`${this.apiUrl}/${id}`, data);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
