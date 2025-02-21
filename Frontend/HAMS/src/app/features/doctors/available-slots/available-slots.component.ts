import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/auth.service';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorAvailabilitySlot } from '../../../shared/models/doctoravailabilityslot.model';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DoctorSlotsDTO } from '../../../shared/models/doctor.model';

@Component({
  selector: 'app-available-slots',
  imports: [CommonModule, FormsModule],
  templateUrl: './available-slots.component.html',
  styleUrl: './available-slots.component.css'
})
export class AvailableSlotsComponent implements OnInit {

  userId?: number;
  doctorId?: number;
  selectedDate: string = this.getCurrentDate();
  minDate: string = this.getCurrentDate();
  doctorSlots: DoctorSlotsDTO[] = [];
  isLoading: boolean = false;
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.fetchDoctorProfile();
  }

  getCurrentDate(): string {
    const today = new Date();
    return today.toISOString().split('T')[0];
  }

  fetchDoctorProfile(): void {
    if (this.userId) {
      this.doctorService.getDoctorProfile(this.userId).subscribe({
        next: (doctorProfile) => {
          this.doctorId = doctorProfile.doctorId;
          this.fetchAvailableSlots();
        },
        error: (err) => {
          console.error('Error fetching doctor profile:', err);
          this.errorMessage = 'Failed to fetch doctor profile.';
        }
      });
    }
  }

  fetchAvailableSlots(): void {
    if (!this.doctorId || !this.selectedDate) return;

    this.isLoading = true;
    this.errorMessage = null;
    const formattedDate = this.selectedDate.toString().split('T')[0];
    console.log('Formatted date sent to server:', formattedDate);

    this.doctorService.getDoctorSlots(this.doctorId, formattedDate).subscribe({
      next: (slots) => {
        this.doctorSlots = slots;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching available slots:', err);
        this.errorMessage = 'Failed to fetch available slots.';
        this.isLoading = false;
      }
    });
  }

  onDateChange(): void {
    this.fetchAvailableSlots();
  }
}
