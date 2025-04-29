import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import {
  ReservationService,
  Reservation,
} from '../../shared/services/reservation.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reservations-list',
  templateUrl: './reservations-list.component.html',
  styleUrls: ['./reservations-list.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class ReservationsListComponent implements OnInit {
  reservations: Reservation[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private reservationService: ReservationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loading = true;
    this.reservationService.getReservations().subscribe({
      next: (reservations) => {
        this.reservations = reservations;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania rezerwacji.';
        this.loading = false;
      },
    });
  }

  goToDetails(reservation: Reservation) {
    this.router.navigate(['/reservations', reservation.id]);
  }
}
