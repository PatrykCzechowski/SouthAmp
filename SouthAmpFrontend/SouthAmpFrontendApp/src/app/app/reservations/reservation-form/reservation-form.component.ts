import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  ReservationService,
  Reservation,
} from '../../shared/services/reservation.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reservation-form',
  templateUrl: './reservation-form.component.html',
  styleUrls: ['./reservation-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ReservationFormComponent implements OnInit {
  @Input() reservation: Reservation | null = null;
  reservationForm: FormGroup;
  loading = false;
  error: string | null = null;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private reservationService: ReservationService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.reservationForm = this.fb.group({
      roomId: ['', Validators.required],
      hotelId: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      totalPrice: [0, [Validators.required, Validators.min(0)]],
      discountCode: [''],
      discountInfo: [null],
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loading = true;
      this.reservationService.getReservation(Number(id)).subscribe({
        next: (reservation) => {
          this.reservation = reservation;
          this.reservationForm.patchValue(reservation);
          this.loading = false;
        },
        error: () => {
          this.error = 'Błąd podczas ładowania rezerwacji.';
          this.loading = false;
        },
      });
    }
  }

  onSubmit() {
    if (this.reservationForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = this.reservationForm.value;
    if (this.isEdit && this.reservation) {
      this.reservationService
        .updateReservation(this.reservation.id, data)
        .subscribe({
          next: () => {
            this.router.navigate(['/reservations']);
          },
          error: () => {
            this.error = 'Błąd podczas zapisywania rezerwacji.';
            this.loading = false;
          },
        });
    } else {
      this.reservationService.createReservation(data).subscribe({
        next: () => {
          this.router.navigate(['/reservations']);
        },
        error: () => {
          this.error = 'Błąd podczas zapisywania rezerwacji.';
          this.loading = false;
        },
      });
    }
  }

  validateDiscount() {
    const code = this.reservationForm.get('discountCode')?.value;
    if (!code) {
      this.reservationForm.patchValue({ discountInfo: null });
      return;
    }
    this.loading = true;
    this.error = null;
    this.reservationForm.patchValue({ discountInfo: null });
    // Pobierz DiscountService dynamicznie, by nie łamać DI
    import('../../discounts/discount.service').then(({ DiscountService }) => {
      const discountService = new DiscountService(
        this.reservationService['http']
      );
      discountService.validateCode(code).subscribe({
        next: (discount) => {
          this.reservationForm.patchValue({ discountInfo: discount });
          // Przelicz cenę
          const basePrice = this.reservationForm.get('totalPrice')?.value || 0;
          const newPrice = basePrice * (1 - discount.percent / 100);
          this.reservationForm.patchValue({ totalPrice: newPrice });
          this.loading = false;
        },
        error: () => {
          this.error = 'Nieprawidłowy kod rabatowy.';
          this.reservationForm.patchValue({ discountInfo: null });
          this.loading = false;
        },
      });
    });
  }
}
