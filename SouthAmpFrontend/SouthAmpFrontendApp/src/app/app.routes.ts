import { Routes } from '@angular/router';
import { LoginComponent } from './app/auth/login/login.component';
import { RegisterComponent } from './app/auth/register/register.component';
import { ResetPasswordComponent } from './app/auth/reset-password/reset-password.component';
import { ProfileComponent } from './app/user/profile/profile.component';
import { HotelsListComponent } from './app/hotels/hotels-list/hotels-list.component';
import { HotelDetailsComponent } from './app/hotels/hotel-details/hotel-details.component';
import { HotelFormComponent } from './app/hotels/hotel-form/hotel-form.component';
import { RoomsListComponent } from './app/rooms/rooms-list/rooms-list.component';
import { RoomDetailsComponent } from './app/rooms/room-details/room-details.component';
import { RoomFormComponent } from './app/rooms/room-form/room-form.component';
import { ReservationsListComponent } from './app/reservations/reservations-list/reservations-list.component';
import { ReservationDetailsComponent } from './app/reservations/reservation-details/reservation-details.component';
import { ReservationFormComponent } from './app/reservations/reservation-form/reservation-form.component';
import { PaymentsListComponent } from './app/payments/payments-list/payments-list.component';
import { PaymentDetailsComponent } from './app/payments/payment-details/payment-details.component';
import { PaymentFormComponent } from './app/payments/payment-form/payment-form.component';
import { ReviewsListComponent } from './app/reviews/reviews-list/reviews-list.component';
import { ReviewFormComponent } from './app/reviews/review-form/review-form.component';
import { ReviewDetailsComponent } from './app/reviews/review-details/review-details.component';
import { AdminPanelComponent } from './app/admin/admin-panel/admin-panel.component';
import { UsersListComponent } from './app/admin/users-list/users-list.component';
import { HotelsModerationComponent } from './app/admin/hotels-moderation/hotels-moderation.component';
import { ReviewsModerationComponent } from './app/admin/reviews-moderation/reviews-moderation.component';
import { TransactionsListComponent } from './app/admin/transactions-list/transactions-list.component';
import { AdminGuard } from './app/shared/services/admin.guard';
import { ReportsListComponent } from './app/reports/reports-list/reports-list.component';
import { ReportFormComponent } from './app/reports/report-form/report-form.component';
import { ReportDetailsComponent } from './app/reports/report-details/report-details.component';
import { DiscountsListComponent } from './app/discounts/discounts-list/discounts-list.component';
import { DiscountFormComponent } from './app/discounts/discount-form/discount-form.component';
import { DiscountDetailsComponent } from './app/discounts/discount-details/discount-details.component';

export const appRoutes: Routes = [
  {
    path: 'auth/login',
    component: LoginComponent,
  },
  {
    path: 'auth/register',
    component: RegisterComponent,
  },
  {
    path: 'auth/reset-password',
    component: ResetPasswordComponent,
  },
  {
    path: 'user/profile',
    component: ProfileComponent,
  },
  {
    path: 'hotels',
    component: HotelsListComponent,
  },
  {
    path: 'hotels/:id',
    component: HotelDetailsComponent,
  },
  {
    path: 'hotels/new',
    component: HotelFormComponent,
  },
  {
    path: 'hotels/:id/edit',
    component: HotelFormComponent,
  },
  {
    path: 'rooms',
    component: RoomsListComponent,
  },
  {
    path: 'rooms/new',
    component: RoomFormComponent,
  },
  {
    path: 'rooms/:id',
    component: RoomDetailsComponent,
  },
  {
    path: 'rooms/:id/edit',
    component: RoomFormComponent,
  },
  {
    path: 'reservations',
    component: ReservationsListComponent,
  },
  {
    path: 'reservations/new',
    component: ReservationFormComponent,
  },
  {
    path: 'reservations/:id',
    component: ReservationDetailsComponent,
  },
  {
    path: 'reservations/:id/edit',
    component: ReservationFormComponent,
  },
  {
    path: 'payments',
    component: PaymentsListComponent,
  },
  {
    path: 'payments/new',
    component: PaymentFormComponent,
  },
  {
    path: 'payments/:id',
    component: PaymentDetailsComponent,
  },
  {
    path: 'payments/:id/edit',
    component: PaymentFormComponent,
  },
  {
    path: 'reviews',
    component: ReviewsListComponent,
  },
  {
    path: 'reviews/new',
    component: ReviewFormComponent,
  },
  {
    path: 'reviews/:id',
    component: ReviewDetailsComponent,
  },
  {
    path: 'reviews/:id/edit',
    component: ReviewFormComponent,
  },
  {
    path: 'reports',
    component: ReportsListComponent,
  },
  {
    path: 'reports/new',
    component: ReportFormComponent,
  },
  {
    path: 'reports/:id',
    component: ReportDetailsComponent,
  },
  {
    path: 'discounts',
    component: DiscountsListComponent,
  },
  {
    path: 'discounts/new',
    component: DiscountFormComponent,
  },
  {
    path: 'discounts/:id',
    component: DiscountDetailsComponent,
  },
  {
    path: 'discounts/:id/edit',
    component: DiscountFormComponent,
  },
  {
    path: 'admin',
    component: AdminPanelComponent,
    canActivate: [AdminGuard],
    children: [
      { path: 'users', component: UsersListComponent },
      { path: 'hotels', component: HotelsModerationComponent },
      { path: 'reviews', component: ReviewsModerationComponent },
      { path: 'transactions', component: TransactionsListComponent },
      { path: '', redirectTo: 'users', pathMatch: 'full' },
    ],
  },
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: '**', redirectTo: 'auth/login' },
];
