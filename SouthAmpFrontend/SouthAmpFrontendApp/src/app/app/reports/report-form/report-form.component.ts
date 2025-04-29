import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from '../report.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-report-form',
  templateUrl: './report-form.component.html',
  styleUrls: ['./report-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ReportFormComponent {
  reportForm: FormGroup;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    private router: Router
  ) {
    this.reportForm = this.fb.group({
      type: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.reportForm.invalid) return;
    this.loading = true;
    this.error = null;
    this.reportService.createReport(this.reportForm.value).subscribe({
      next: () => {
        this.router.navigate(['/reports']);
      },
      error: () => {
        this.error = 'Nie udało się wysłać zgłoszenia.';
        this.loading = false;
      },
    });
  }
}
