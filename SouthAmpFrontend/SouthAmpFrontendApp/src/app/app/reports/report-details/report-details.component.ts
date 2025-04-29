import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReportService, Report } from '../report.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-report-details',
  templateUrl: './report-details.component.html',
  styleUrls: ['./report-details.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class ReportDetailsComponent implements OnInit {
  report: Report | null = null;
  loading = false;
  error: string | null = null;
  responseForm: FormGroup;
  responseSuccess = false;

  constructor(
    private route: ActivatedRoute,
    private reportService: ReportService,
    private fb: FormBuilder
  ) {
    this.responseForm = this.fb.group({
      adminResponse: [''],
    });
  }

  ngOnInit() {
    this.loading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.reportService.getReport(id).subscribe({
      next: (report) => {
        this.report = report;
        this.responseForm.patchValue({
          adminResponse: report.adminResponse || '',
        });
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania szczegółów zgłoszenia.';
        this.loading = false;
      },
    });
  }

  sendResponse() {
    if (!this.report) return;
    this.loading = true;
    this.reportService
      .updateReport(this.report.id, {
        adminResponse: this.responseForm.value.adminResponse,
        status: 'closed',
      })
      .subscribe({
        next: (updated) => {
          this.report = updated;
          this.responseSuccess = true;
          this.loading = false;
        },
        error: () => {
          this.error = 'Nie udało się wysłać odpowiedzi.';
          this.loading = false;
        },
      });
  }
}
