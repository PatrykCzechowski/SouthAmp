import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';

@Component({
  selector: 'app-reports-list',
  templateUrl: './reports-list.component.html',
  styleUrls: ['./reports-list.component.scss'],
  standalone: true,
  imports: [CommonModule, TranslateModule],
})
export class ReportsListComponent implements OnInit {
  reports: any[] = [];
  loading = false;
  error: string | null = null;

  constructor(private reportService: ReportService) {}

  ngOnInit() {
    this.loading = true;
    this.reportService.getReports().subscribe({
      next: (reports) => {
        this.reports = reports;
        this.loading = false;
      },
      error: () => {
        this.error = 'Błąd ładowania zgłoszeń.';
        this.loading = false;
      },
    });
  }
}
