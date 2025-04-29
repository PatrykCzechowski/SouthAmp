import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DiscountService, Discount } from '../discount.service';

@Component({
  selector: 'app-discounts-list',
  templateUrl: './discounts-list.component.html',
  styleUrls: ['./discounts-list.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class DiscountsListComponent implements OnInit {
  discounts: Discount[] = [];
  loading = false;
  error: string | null = null;

  constructor(private discountService: DiscountService) {}

  ngOnInit() {
    this.loading = true;
    this.discountService.getDiscounts().subscribe({
      next: (discounts) => {
        this.discounts = discounts;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania kodów rabatowych.';
        this.loading = false;
      },
    });
  }
}
