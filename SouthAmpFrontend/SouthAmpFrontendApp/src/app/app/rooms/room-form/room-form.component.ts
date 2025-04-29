import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RoomService, Room } from '../../shared/services/room.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-room-form',
  templateUrl: './room-form.component.html',
  styleUrls: ['./room-form.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
})
export class RoomFormComponent implements OnInit {
  @Input() room: Room | null = null;
  roomForm: FormGroup;
  loading = false;
  error: string | null = null;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private roomService: RoomService,
    private router: Router,
    private route: ActivatedRoute,
    public translate: TranslateService
  ) {
    this.roomForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      available: [true],
      images: [''], // comma-separated URLs
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loading = true;
      this.roomService.getRoom(Number(id)).subscribe({
        next: (room) => {
          this.room = room;
          this.roomForm.patchValue({
            ...room,
            images: room.images ? room.images.join(',') : '',
          });
          this.loading = false;
        },
        error: () => {
          this.error = this.translate.instant('ROOMS.FORM_LOAD_ERROR');
          this.loading = false;
        },
      });
    }
  }

  onSubmit() {
    if (this.roomForm.invalid) return;
    this.loading = true;
    this.error = null;
    const data = {
      ...this.roomForm.value,
      images: this.roomForm.value.images
        ? this.roomForm.value.images.split(',').map((s: string) => s.trim())
        : [],
    };
    if (this.isEdit && this.room) {
      this.roomService.updateRoom(this.room.id, data).subscribe({
        next: () => {
          this.router.navigate(['/rooms']);
        },
        error: () => {
          this.error = this.translate.instant('ROOMS.FORM_SAVE_ERROR');
          this.loading = false;
        },
      });
    } else {
      this.roomService.createRoom(data).subscribe({
        next: () => {
          this.router.navigate(['/rooms']);
        },
        error: () => {
          this.error = this.translate.instant('ROOMS.FORM_SAVE_ERROR');
          this.loading = false;
        },
      });
    }
  }
}
