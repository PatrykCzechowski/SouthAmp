import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DiscountService, Discount } from '../discount.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-discount-form',
  templateUrl: './discount-form.component.html',
  styleUrls: ['./discount-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class DiscountFormComponent implements OnInit {
  discountForm: FormGroup;
  loading = false;
  error: string | null = null;
  isEdit = false;
  discountId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private discountService: DiscountService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.discountForm = this.fb.group({
      code: ['', Validators.required],
      percent: [
        0,
        [Validators.required, Validators.min(1), Validators.max(100)],
      ],
      validFrom: ['', Validators.required],
      validTo: ['', Validators.required],
      usageLimit: [1, [Validators.required, Validators.min(1)]],
      active: [true],
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.discountId = Number(id);
      this.loading = true;
      this.discountService.getDiscount(this.discountId).subscribe({
        next: (discount) => {
          this.discountForm.patchValue(discount);
          this.loading = false;
        },
        error: () => {
          this.error = 'Błąd ładowania kodu rabatowego.';
          this.loading = false;
        },
      });
    }
  }

  onSubmit() {
    if (this.discountForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = this.discountForm.value;
    if (this.isEdit && this.discountId) {
      this.discountService.updateDiscount(this.discountId, data).subscribe({
        next: () => this.router.navigate(['/discounts']),
        error: () => {
          this.error = 'Nie udało się zapisać zmian.';
          this.loading = false;
        },
      });
    } else {
      this.discountService.createDiscount(data).subscribe({
        next: () => this.router.navigate(['/discounts']),
        error: () => {
          this.error = 'Nie udało się utworzyć kodu.';
          this.loading = false;
        },
      });
    }
  }
}
