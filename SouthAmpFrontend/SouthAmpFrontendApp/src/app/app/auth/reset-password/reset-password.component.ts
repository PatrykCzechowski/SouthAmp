import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  loading = false;
  success: boolean = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    public translate: TranslateService
  ) {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.resetForm.invalid) return;
    this.loading = true;
    this.error = null;
    this.success = false;
    const { email } = this.resetForm.value;
    this.authService.resetPassword(email).subscribe({
      next: () => {
        this.success = true;
        this.loading = false;
      },
      error: () => {
        this.error = this.translate.instant('RESET.ERROR');
        this.loading = false;
      },
    });
  }
}
