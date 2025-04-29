import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ReservationFormComponent } from './reservation-form/reservation-form.component';
import { ReservationsListComponent } from './reservations-list/reservations-list.component';
import { ReservationDetailsComponent } from './reservation-details/reservation-details.component';

@NgModule({
  imports: [
    SharedModule,
    ReservationFormComponent,
    ReservationsListComponent,
    ReservationDetailsComponent,
  ],
})
export class ReservationsModule {}
