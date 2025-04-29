import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { PaymentService, Payment } from '../../shared/services/payment.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payments-list',
  templateUrl: './payments-list.component.html',
  styleUrls: ['./payments-list.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class PaymentsListComponent implements OnInit {
  payments: Payment[] = [];
  loading = false;
  error: string | null = null;

  constructor(private paymentService: PaymentService, private router: Router) {}

  ngOnInit() {
    this.loading = true;
    this.paymentService.getPayments().subscribe({
      next: (payments) => {
        this.payments = payments;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania płatności.';
        this.loading = false;
      },
    });
  }

  goToDetails(payment: Payment) {
    this.router.navigate(['/payments', payment.id]);
  }
}
