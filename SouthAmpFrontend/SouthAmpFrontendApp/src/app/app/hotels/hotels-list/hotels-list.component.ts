import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { HotelService, Hotel } from '../../shared/services/hotel.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-hotels-list',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule, ReactiveFormsModule],
  templateUrl: './hotels-list.component.html',
  styleUrls: ['./hotels-list.component.scss'],
})
export class HotelsListComponent implements OnInit {
  hotels: Hotel[] = [];
  loading = false;
  error: string | null = null;
  cityFilter = '';
  countryFilter = '';

  constructor(private hotelService: HotelService, private router: Router) {}

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

  goToDetails(hotel: Hotel) {
    this.router.navigate(['/hotels', hotel.id]);
  }

  filterHotels(): Hotel[] {
    return this.hotels.filter(
      (hotel) =>
        (!this.cityFilter ||
          hotel.city?.toLowerCase().includes(this.cityFilter.toLowerCase())) &&
        (!this.countryFilter ||
          hotel.country
            ?.toLowerCase()
            .includes(this.countryFilter.toLowerCase()))
    );
  }
}
