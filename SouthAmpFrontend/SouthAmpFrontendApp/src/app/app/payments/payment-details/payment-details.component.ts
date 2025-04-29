import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaymentService, Payment } from '../../shared/services/payment.service';

@Component({
  selector: 'app-payment-details',
  templateUrl: './payment-details.component.html',
  styleUrls: ['./payment-details.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class PaymentDetailsComponent implements OnInit {
  payment: Payment | null = null;
  loading = false;
  error: string | null = null;
  refundSuccess = false;

  constructor(
    private route: ActivatedRoute,
    private paymentService: PaymentService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.paymentService.getPayment(id).subscribe({
      next: (payment) => {
        this.payment = payment;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów płatności.';
        this.loading = false;
      },
    });
  }

  refundPayment() {
    if (this.payment && confirm('Czy na pewno chcesz zwrócić tę płatność?')) {
      this.paymentService.refundPayment(this.payment.id).subscribe({
        next: (payment) => {
          this.refundSuccess = true;
          this.payment = payment;
        },
        error: () => {
          this.error = 'Nie udało się zwrócić płatności.';
        },
      });
    }
  }
}
