import { Component, OnInit } from '@angular/core';
import { AppointmentCreateDTO } from '../../../shared/models/appointment.model';
import { AppointmentService } from '../../../services/appointment.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorDTO } from '../../../shared/models/doctor.model';
import { DoctorAvailabilitySlot } from '../../../shared/models/doctoravailabilityslot.model';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-appointment-create',
  imports: [CommonModule, FormsModule],
  templateUrl: './create-appointment.component.html',
  styleUrl: './create-appointment.component.css'
})
export class CreateAppointmentComponent implements OnInit {

  doctors: DoctorDTO[] = [];
  availableSlots: DoctorAvailabilitySlot[] = [];
  selectedSlot: DoctorAvailabilitySlot | null = null; 
  appointment: AppointmentCreateDTO = new AppointmentCreateDTO();
  successMessage: string | null = null;
  errorMessage: string | null = null;
  isLoadingSlots = false;
  // patientId?: number 

  constructor(
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.appointment.patientId = this.authService.getUserId()
    this.fetchDoctors();
  }

  fetchDoctors(): void {
    this.doctorService.getAllDoctorProfile().subscribe({
      next: (data) => {
        this.doctors = data;
      },
      error: (err) => {
        console.error('Failed to fetch doctors', err);
      },
    });
  }

  fetchAvailableSlots(): void {
    if (this.appointment.doctorId && this.appointment.date) {
      this.isLoadingSlots = true;
      const formattedDate = this.appointment.date.toString().split('T')[0]; 
      
      this.doctorService
        .getDoctorAvailabilitySlots(this.appointment.doctorId, formattedDate)
        .subscribe({
          next: (slots: DoctorAvailabilitySlot[]) => {
            this.availableSlots = slots;
            this.isLoadingSlots = false;
          },
          error: (err) => {
            console.error('Failed to fetch available slots', err);
            this.isLoadingSlots = false;
          },
        });
    }
  }

  onSlotChange(): void {
    if (this.selectedSlot) {
      this.appointment.startTime = this.selectedSlot.slotStartTime;
      this.appointment.endTime = this.selectedSlot.slotEndTime; 
    }
  }

  createAppointment(): void {
    this.appointmentService.createAppointment(this.appointment).subscribe({
      next: (response: string) => {
        this.successMessage = response;
        this.errorMessage = null;
      },
      error: (err) => {
        this.errorMessage = 'Failed to create appointment.';
        console.error(err);
      },
    });
  }
}
