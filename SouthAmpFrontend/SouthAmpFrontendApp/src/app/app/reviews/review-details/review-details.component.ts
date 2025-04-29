import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReviewService, Review } from '../../shared/services/review.service';

@Component({
  selector: 'app-review-details',
  templateUrl: './review-details.component.html',
  styleUrls: ['./review-details.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class ReviewDetailsComponent implements OnInit {
  review: Review | null = null;
  loading = false;
  error: string | null = null;
  deleteSuccess = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reviewService: ReviewService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.reviewService.getReview(id).subscribe({
      next: (review) => {
        this.review = review;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów opinii.';
        this.loading = false;
      },
    });
  }

  editReview() {
    if (this.review) {
      this.router.navigate(['/reviews', this.review.id, 'edit']);
    }
  }

  deleteReview() {
    if (this.review && confirm('Czy na pewno chcesz usunąć tę opinię?')) {
      this.reviewService.deleteReview(this.review.id).subscribe({
        next: () => {
          this.deleteSuccess = true;
          setTimeout(() => this.router.navigate(['/reviews']), 1000);
        },
        error: () => {
          this.error = 'Nie udało się usunąć opinii.';
        },
      });
    }
  }
}
