import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnDestroy } from '@angular/core';
import { NotificationService, Notification } from '../notification.service';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class SnackbarComponent implements OnDestroy {
  notification: Notification | null = null;
  visible = false;
  private sub: Subscription;
  private timerSub: Subscription | null = null;

  constructor(private notificationService: NotificationService) {
    this.sub = this.notificationService.notifications$.subscribe((n) => {
      this.notification = n;
      this.visible = true;
      if (this.timerSub) this.timerSub.unsubscribe();
      this.timerSub = timer(3000).subscribe(() => (this.visible = false));
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    if (this.timerSub) this.timerSub.unsubscribe();
  }
}
