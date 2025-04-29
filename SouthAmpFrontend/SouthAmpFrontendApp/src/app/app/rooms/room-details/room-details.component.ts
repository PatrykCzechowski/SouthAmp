import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RoomService, Room } from '../../shared/services/room.service';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class RoomDetailsComponent implements OnInit {
  room: Room | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private roomService: RoomService
  ) {}

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.roomService.getRoom(id).subscribe({
      next: (room) => {
        this.room = room;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów pokoju.';
        this.loading = false;
      },
    });
  }

  editRoom() {
    if (this.room) {
      this.router.navigate(['/rooms', this.room.id, 'edit']);
    }
  }

  deleteRoom() {
    if (this.room && confirm('Czy na pewno chcesz usunąć ten pokój?')) {
      this.roomService.deleteRoom(this.room.id).subscribe({
        next: () => this.router.navigate(['/rooms']),
        error: () => (this.error = 'Nie udało się usunąć pokoju.'),
      });
    }
  }
}
