import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/auth.service';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorAvailabilityDTO } from '../../../shared/models/doctor.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctor-availability',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './doctor-availability.component.html',
  styleUrls: ['./doctor-availability.component.css']
})
export class DoctorAvailabilityComponent implements OnInit {

  availabilityForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private doctorService: DoctorService,
    private authService: AuthService
  ) {
    this.availabilityForm = this.fb.group({
      date: ['', Validators.required],
      dayOfWeek: [{ value: '', disabled: true }, Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      slotDurationMinutes: [
        '',
        [Validators.required, Validators.min(1)]
      ],
    });
  }

  ngOnInit(): void {
    // Update Day of Week based on selected Date
    this.availabilityForm.get('date')?.valueChanges.subscribe((selectedDate: string) => {
      if (selectedDate) {
        const dayOfWeek = this.getDayOfWeek(new Date(selectedDate));
        this.availabilityForm.patchValue({ dayOfWeek }); // Update the dayOfWeek field dynamically
      }
    });
  }

  // Helper to get Day of the Week
  getDayOfWeek(date: Date): string {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[date.getDay()];
  }

  submitAvailability(): void {
    if (this.availabilityForm.invalid) {
      this.errorMessage = 'Please fill out all required fields correctly.';
      return;
    }

    const startTime = this.availabilityForm.value.startTime + ":00";
  const endTime = this.availabilityForm.value.endTime + ":00";
  
    const availability: DoctorAvailabilityDTO = {
      doctorId: 0, 
      userId: this.authService.getUserId(),
      date: this.availabilityForm.value.date, 
      startTime: startTime,
      endTime: endTime,
      slotDurationMinutes: this.availabilityForm.value.slotDurationMinutes,
      dayOfWeek: this.getDayOfWeek(new Date(this.availabilityForm.value.date)),
    };
  
    this.doctorService.manageAvailability(availability).subscribe({
      next: (response: string) => {
        this.successMessage = response;
        this.errorMessage = null;
        this.availabilityForm.reset();
      },
      error: (err) => {
        this.errorMessage = 'Failed to update availability. Please try again.';
        this.successMessage = null;
        console.error(err);
      },
    });
  }
  
}

