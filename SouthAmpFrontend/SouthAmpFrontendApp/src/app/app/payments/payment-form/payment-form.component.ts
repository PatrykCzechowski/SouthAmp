import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { PaymentService } from '../../shared/services/payment.service';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-payment-form',
  templateUrl: './payment-form.component.html',
  styleUrls: ['./payment-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class PaymentFormComponent {
  paymentForm: FormGroup;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private paymentService: PaymentService,
    private router: Router,
    public translate: TranslateService
  ) {
    this.paymentForm = this.fb.group({
      reservationId: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(1)]],
    });
  }

  onSubmit() {
    if (this.paymentForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = this.paymentForm.value;
    this.paymentService.createPayment(data).subscribe({
      next: () => {
        this.router.navigate(['/payments']);
      },
      error: () => {
        this.error = this.translate.instant('PAYMENTS.FORM_SAVE_ERROR');
        this.loading = false;
      },
    });
  }
}
