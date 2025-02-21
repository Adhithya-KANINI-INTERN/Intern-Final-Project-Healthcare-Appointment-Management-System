import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../services/appointment.service';
import { AppointmentApprovalDTO, AppointmentCompletedDTO, AppointmentDTO, AppointmentUpdateDTO } from '../../../shared/models/appointment.model';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/auth.service';
import { AllAppointmentsComponent } from "../../appointments/all-appointments/all-appointments.component";

@Component({
  selector: 'app-patientappointments',
  imports: [CommonModule, AllAppointmentsComponent],
  templateUrl: './patientappointments.component.html',
  styleUrl: './patientappointments.component.css'
})
export class PatientappointmentsComponent implements OnInit{


  appointments: AppointmentDTO[] = [];
  errorMessage: string | null = null;
  userRole: string | null = null;
  userId: number | null = null;

  constructor(
    private appointmentService: AppointmentService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole(); 
    this.userId = this.authService.getUserId();
    this.fetchAppointments(this.userId);
  }

  fetchAppointments(userId : number): void {
    if (this.userRole === 'Doctor') {
      this.appointmentService.getAppointmentsbyDoctor(userId).subscribe({
        next: (data) => {
          this.appointments = data;
          this.errorMessage = null;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to load doctor appointments.';
        },
      });
    } else if (this.userRole === 'Patient') {
      this.appointmentService.getPatientAppointments(userId).subscribe({
        next: (data) => {
          this.appointments = data;
          this.errorMessage = null;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to load patient appointments.';
        },
      });
    }
  }

  MarkAppointmentAsCompleted(appointmentId: number): void {
    this.router.navigate(['/approve-appointment', {appointmentId }]);
  }

  navigateToupdateAppointment(appointmentId : number): void {
    this.router.navigate(['/update-appointment', {appointmentId }]);
  }

   navigateToCancelAppointment(appointmentId: number): void {
   this.router.navigate(['/cancel-appointment', {appointmentId}]);
  }

  navigateToCreateAppointment(): void {
    this.router.navigate(['/create-appointment']);
  }
}
