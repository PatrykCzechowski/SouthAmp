import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  imports: [SharedModule, ProfileComponent],
})
export class UserModule {}
