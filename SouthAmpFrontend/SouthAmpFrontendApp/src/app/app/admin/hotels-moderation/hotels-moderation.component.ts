import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HotelService, Hotel } from '../../shared/services/hotel.service';

@Component({
  selector: 'app-hotels-moderation',
  templateUrl: './hotels-moderation.component.html',
  styleUrls: ['./hotels-moderation.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class HotelsModerationComponent implements OnInit {
  hotels: Hotel[] = [];
  loading = false;
  error: string | null = null;

  constructor(private hotelService: HotelService) {}

  ngOnInit() {
    this.loading = true;
    this.hotelService.getHotels().subscribe({
      next: (hotels) => {
        this.hotels = hotels;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania hoteli.';
        this.loading = false;
      },
    });
  }

  approveHotel(hotelId: number) {
    // TODO: implementacja akceptacji hotelu
  }
  rejectHotel(hotelId: number) {
    // TODO: implementacja odrzucenia hotelu
  }
  deleteHotel(hotelId: number) {
    // TODO: implementacja usuwania hotelu
  }
}
