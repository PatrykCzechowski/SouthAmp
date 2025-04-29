import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ReviewService, Review } from '../../shared/services/review.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ReviewFormComponent implements OnInit {
  @Input() review: Review | null = null;
  reviewForm: FormGroup;
  loading = false;
  error: string | null = null;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private reviewService: ReviewService,
    private router: Router,
    private route: ActivatedRoute,
    public translate: TranslateService
  ) {
    this.reviewForm = this.fb.group({
      hotelId: ['', Validators.required],
      reservationId: ['', Validators.required],
      rating: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
      comment: ['', Validators.required],
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loading = true;
      this.reviewService.getReview(Number(id)).subscribe({
        next: (review) => {
          this.review = review;
          this.reviewForm.patchValue(review);
          this.loading = false;
        },
        error: () => {
          this.error = this.translate.instant('REVIEWS.FORM_LOAD_ERROR');
          this.loading = false;
        },
      });
    }
  }

  onSubmit() {
    if (this.reviewForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = this.reviewForm.value;
    if (this.isEdit && this.review) {
      this.reviewService.updateReview(this.review.id, data).subscribe({
        next: () => {
          this.router.navigate(['/reviews']);
        },
        error: () => {
          this.error = this.translate.instant('REVIEWS.FORM_SAVE_ERROR');
          this.loading = false;
        },
      });
    } else {
      this.reviewService.createReview(data).subscribe({
        next: () => {
          this.router.navigate(['/reviews']);
        },
        error: () => {
          this.error = this.translate.instant('REVIEWS.FORM_SAVE_ERROR');
          this.loading = false;
        },
      });
    }
  }
}
