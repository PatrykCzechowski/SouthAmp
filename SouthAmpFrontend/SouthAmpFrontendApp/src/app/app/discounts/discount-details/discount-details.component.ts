import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DiscountService, Discount } from '../discount.service';

@Component({
  selector: 'app-discount-details',
  templateUrl: './discount-details.component.html',
  styleUrls: ['./discount-details.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class DiscountDetailsComponent implements OnInit {
  discount: Discount | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private discountService: DiscountService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.discountService.getDiscount(id).subscribe({
      next: (discount) => {
        this.discount = discount;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów kodu rabatowego.';
        this.loading = false;
      },
    });
  }
}
