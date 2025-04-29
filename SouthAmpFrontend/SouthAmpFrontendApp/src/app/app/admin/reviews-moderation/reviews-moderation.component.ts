import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReviewService, Review } from '../../shared/services/review.service';

@Component({
  selector: 'app-reviews-moderation',
  templateUrl: './reviews-moderation.component.html',
  styleUrls: ['./reviews-moderation.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class ReviewsModerationComponent implements OnInit {
  reviews: Review[] = [];
  loading = false;
  error: string | null = null;

  constructor(private reviewService: ReviewService) {}

  ngOnInit() {
    this.loading = true;
    this.reviewService.getReviews().subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania opinii.';
        this.loading = false;
      },
    });
  }

  approveReview(reviewId: number) {
    // TODO: implementacja akceptacji opinii
  }
  rejectReview(reviewId: number) {
    // TODO: implementacja odrzucenia opinii
  }
  deleteReview(reviewId: number) {
    // TODO: implementacja usuwania opinii
  }
}
