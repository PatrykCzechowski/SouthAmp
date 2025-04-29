import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HotelService, Hotel } from '../../shared/services/hotel.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-hotel-form',
  templateUrl: './hotel-form.component.html',
  styleUrls: ['./hotel-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class HotelFormComponent implements OnInit, AfterViewInit {
  @Input() hotel: Hotel | null = null;
  hotelForm: FormGroup;
  loading = false;
  error: string | null = null;
  isEdit = false;
  map: any = null;
  marker: any = null;

  constructor(
    private fb: FormBuilder,
    private hotelService: HotelService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.hotelForm = this.fb.group({
      name: ['', Validators.required],
      location: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      lat: [null, Validators.required],
      lng: [null, Validators.required],
      description: ['', Validators.required],
      imageUrl: [''],
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loading = true;
      this.hotelService.getHotel(Number(id)).subscribe({
        next: (hotel) => {
          this.hotel = hotel;
          this.hotelForm.patchValue(hotel);
          this.loading = false;
        },
        error: () => {
          this.error = 'Błąd podczas ładowania hotelu';
          this.loading = false;
        },
      });
    }
  }

  ngAfterViewInit() {
    if (typeof window === 'undefined') return; // SSR guard
    import('leaflet').then(L => {
      setTimeout(() => {
        if (!this.map) {
          this.map = L.map('hotelMap').setView([52.2297, 21.0122], 6);
          L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors',
          }).addTo(this.map);
          this.map.on('click', (e: any) => {
            const lat = e.latlng.lat;
            const lng = e.latlng.lng;
            this.hotelForm.patchValue({ lat, lng });
            if (this.marker) {
              this.marker.setLatLng([lat, lng]);
            } else {
              this.marker = L.marker([lat, lng]).addTo(this.map!);
            }
          });
        }
        const lat = this.hotelForm.get('lat')?.value;
        const lng = this.hotelForm.get('lng')?.value;
        if (lat && lng && this.map) {
          this.marker = L.marker([lat, lng]).addTo(this.map);
          this.map.setView([lat, lng], 12);
        }
      }, 0);
    });
  }

  onSubmit() {
    if (this.hotelForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = this.hotelForm.value;
    if (this.isEdit && this.hotel) {
      this.hotelService.updateHotel(this.hotel.id, data).subscribe({
        next: () => {
          this.router.navigate(['/hotels']);
        },
        error: () => {
          this.error = 'Błąd podczas zapisywania hotelu';
          this.loading = false;
        },
      });
    } else {
      this.hotelService.createHotel(data).subscribe({
        next: () => {
          this.router.navigate(['/hotels']);
        },
        error: () => {
          this.error = 'Błąd podczas zapisywania hotelu';
          this.loading = false;
        },
      });
    }
  }
}
