import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { UserService } from '../../shared/services/user.service';
import { AuthService, User } from '../../shared/services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ProfileComponent {
  profileForm: FormGroup;
  passwordForm: FormGroup;
  loading = false;
  passwordLoading = false;
  success: boolean = false;
  passwordSuccess: boolean = false;
  error: string | null = null;
  passwordError: string | null = null;
  user: User | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    public translate: TranslateService
  ) {
    this.user = this.authService.getUser();
    this.profileForm = this.fb.group({
      username: [this.user?.username || '', Validators.required],
      email: [this.user?.email || '', [Validators.required, Validators.email]],
    });
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onProfileSubmit() {
    if (this.profileForm.invalid) return;
    this.loading = true;
    this.error = null;
    const { username, email } = this.profileForm.value;
    this.userService.updateProfile({ username, email }).subscribe({
      next: (user) => {
        this.success = true;
        this.loading = false;
        this.user = user;
      },
      error: () => {
        this.error = this.translate.instant('PROFILE.ERROR');
        this.loading = false;
      },
    });
  }

  onPasswordSubmit() {
    if (this.passwordForm.invalid) return;
    this.passwordLoading = true;
    this.passwordError = null;
    const { currentPassword, newPassword } = this.passwordForm.value;
    this.userService.changePassword(currentPassword, newPassword).subscribe({
      next: () => {
        this.passwordSuccess = true;
        this.passwordLoading = false;
      },
      error: () => {
        this.passwordError = this.translate.instant('PROFILE.PASSWORD_ERROR');
        this.passwordLoading = false;
      },
    });
  }
}
