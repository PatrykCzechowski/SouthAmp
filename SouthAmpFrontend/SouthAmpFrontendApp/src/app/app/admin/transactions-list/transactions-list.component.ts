import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { PaymentService, Payment } from '../../shared/services/payment.service';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class TransactionsListComponent implements OnInit {
  payments: Payment[] = [];
  loading = false;
  error: string | null = null;

  constructor(private paymentService: PaymentService) {}

  ngOnInit() {
    this.loading = true;
    this.paymentService.getPayments().subscribe({
      next: (payments) => {
        this.payments = payments;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania transakcji.';
        this.loading = false;
      },
    });
  }

  refundPayment(paymentId: number) {
    // TODO: implementacja refundacji płatności
  }
}
