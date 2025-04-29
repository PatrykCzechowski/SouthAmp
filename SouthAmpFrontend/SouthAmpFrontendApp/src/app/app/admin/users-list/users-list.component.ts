import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { User } from '../../shared/services/auth.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class UsersListComponent implements OnInit {
  users: User[] = [];
  loading = false;
  error: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.loading = true;
    this.userService.getUsers().subscribe({
      next: (users: User[]) => {
        this.users = users;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania użytkowników.';
        this.loading = false;
      },
    });
  }

  // Przykładowe akcje admina
  banUser(userId: number) {
    // TODO: implementacja banowania
  }
  activateUser(userId: number) {
    // TODO: implementacja aktywacji
  }
  deleteUser(userId: number) {
    // TODO: implementacja usuwania
  }
}
