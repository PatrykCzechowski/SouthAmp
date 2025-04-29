import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ReviewFormComponent } from './review-form/review-form.component';
import { ReviewsListComponent } from './reviews-list/reviews-list.component';
import { ReviewDetailsComponent } from './review-details/review-details.component';

@NgModule({
  imports: [
    SharedModule,
    ReviewFormComponent,
    ReviewsListComponent,
    ReviewDetailsComponent,
  ],
})
export class ReviewsModule {}
