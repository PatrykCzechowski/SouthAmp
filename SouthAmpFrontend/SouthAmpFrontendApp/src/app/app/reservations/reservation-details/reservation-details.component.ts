import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ReservationService,
  Reservation,
} from '../../shared/services/reservation.service';

@Component({
  selector: 'app-reservation-details',
  templateUrl: './reservation-details.component.html',
  styleUrls: ['./reservation-details.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class ReservationDetailsComponent implements OnInit {
  reservation: Reservation | null = null;
  loading = false;
  error: string | null = null;
  cancelSuccess = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reservationService: ReservationService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.reservationService.getReservation(id).subscribe({
      next: (reservation) => {
        this.reservation = reservation;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów rezerwacji.';
        this.loading = false;
      },
    });
  }

  cancelReservation() {
    if (
      this.reservation &&
      confirm('Czy na pewno chcesz anulować tę rezerwację?')
    ) {
      this.reservationService.cancelReservation(this.reservation.id).subscribe({
        next: () => {
          this.cancelSuccess = true;
          this.reservation!.status = 'cancelled';
        },
        error: () => {
          this.error = 'Nie udało się anulować rezerwacji.';
        },
      });
    }
  }
}
