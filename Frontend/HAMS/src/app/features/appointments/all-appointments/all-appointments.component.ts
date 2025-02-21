import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../services/appointment.service';
import { AppointmentDTO } from '../../../shared/models/appointment.model';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-all-appointments',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './all-appointments.component.html',
  styleUrl: './all-appointments.component.css'
})
export class AllAppointmentsComponent implements OnInit {

  appointmentDTO: AppointmentDTO[] = [];
  errorMessage: string | null = null;

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.fetchAllAppointments();
  }


  fetchAllAppointments() : void {
    this.appointmentService.getAllAppointments().subscribe({
      next: (data) => {
        this.appointmentDTO = data,
        this.errorMessage = null;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load apppointments.';
      },
    })
  }

}
