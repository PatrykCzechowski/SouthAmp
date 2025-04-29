import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { RoomService, Room } from '../../shared/services/room.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-rooms-list',
  templateUrl: './rooms-list.component.html',
  styleUrls: ['./rooms-list.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class RoomsListComponent implements OnInit {
  rooms: Room[] = [];
  loading = false;
  error: string | null = null;

  constructor(private roomService: RoomService, private router: Router) {}

  ngOnInit() {
    this.loading = true;
    this.roomService.getRooms().subscribe({
      next: (rooms) => {
        this.rooms = rooms;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania pokoi.';
        this.loading = false;
      },
    });
  }

  goToDetails(room: Room) {
    this.router.navigate(['/rooms', room.id]);
  }
}
