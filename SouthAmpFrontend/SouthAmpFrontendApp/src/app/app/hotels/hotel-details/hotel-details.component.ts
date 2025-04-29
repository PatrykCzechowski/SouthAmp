import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HotelService, Hotel } from '../../shared/services/hotel.service';

@Component({
  selector: 'app-hotel-details',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule, ReactiveFormsModule],
  templateUrl: './hotel-details.component.html',
  styleUrls: ['./hotel-details.component.scss'],
})
export class HotelDetailsComponent implements OnInit {
  hotel: Hotel | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private hotelService: HotelService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.hotelService.getHotel(id).subscribe({
      next: (hotel) => {
        this.hotel = hotel;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów hotelu.';
        this.loading = false;
      },
    });
  }
}
