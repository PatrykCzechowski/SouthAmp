import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { RoomFormComponent } from './room-form/room-form.component';
import { RoomsListComponent } from './rooms-list/rooms-list.component';
import { RoomDetailsComponent } from './room-details/room-details.component';

@NgModule({
  imports: [
    SharedModule,
    RoomFormComponent,
    RoomsListComponent,
    RoomDetailsComponent,
  ],
})
export class RoomsModule {}
