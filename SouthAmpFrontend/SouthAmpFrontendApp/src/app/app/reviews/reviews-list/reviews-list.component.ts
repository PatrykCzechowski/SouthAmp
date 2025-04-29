import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ReviewService, Review } from '../../shared/services/review.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reviews-list',
  templateUrl: './reviews-list.component.html',
  styleUrls: ['./reviews-list.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class ReviewsListComponent implements OnInit {
  reviews: Review[] = [];
  loading = false;
  error: string | null = null;
  averageRating: number | null = null;

  constructor(private reviewService: ReviewService, private router: Router) {}

  ngOnInit() {
    this.loading = true;
    this.reviewService.getReviews().subscribe({
      next: (reviews) => {
        this.reviews = reviews;
        this.loading = false;
        if (reviews.length) {
          this.averageRating =
            reviews.reduce((sum, r) => sum + r.rating, 0) / reviews.length;
        }
      },
      error: () => {
        this.error = 'Błąd ładowania opinii.';
        this.loading = false;
      },
    });
  }

  goToDetails(review: Review) {
    this.router.navigate(['/reviews', review.id]);
  }
}
