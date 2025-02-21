import { Component, OnInit } from '@angular/core';
import { AppointmentDTO } from '../../../shared/models/appointment.model';
import { AppointmentService } from '../../../services/appointment.service';
import { AuthService } from '../../../core/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctors-appointments',
  imports: [CommonModule],
  templateUrl: './doctors-appointments.component.html',
  styleUrl: './doctors-appointments.component.css'
})
export class DoctorsAppointmentsComponent implements OnInit {

  userId?: number; 
  appointments: AppointmentDTO[] = [];
  errorMessage: string | null = null;

  constructor(private appointmentService: AppointmentService, private authService: AuthService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    if (this.userId !== undefined) {
      this.fetchAppointments(this.userId);
    } else {
      this.errorMessage = 'User ID is undefined';
    }
  }

  fetchAppointments(userId: number): void {
    this.appointmentService.getAppointmentsbyDoctor(userId).subscribe({
      next: (data) => {
        this.appointments = data;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load appointments';
        console.error(err);
      },
    });
  }
}
