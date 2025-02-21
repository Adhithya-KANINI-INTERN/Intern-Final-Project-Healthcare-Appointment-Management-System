import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../services/appointment.service';
import { AppointmentUpdateDTO } from '../../../shared/models/appointment.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-update-appointment',
  imports: [CommonModule, FormsModule],
  templateUrl: './update-appointment.component.html',
  styleUrls: ['./update-appointment.component.css']
})
export class UpdateAppointmentComponent implements OnInit {
  appointmentUpdate: AppointmentUpdateDTO = {
    appointmentId: 0,
    newDate: '',
    newStartTime: '',
    newEndTime: '',
    reason: '',
  };
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(private appointmentService: AppointmentService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const appointmentId = params.get('appointmentId');
      if (appointmentId) {
        this.appointmentUpdate.appointmentId = +appointmentId;
      }
    });
  }

  updateAppointment(): void {
    this.appointmentService.updateAppointment(this.appointmentUpdate).subscribe({
      next: (response: string) => {
        this.successMessage = response;
        this.errorMessage = null;
      },
      error: (err) => {
        this.errorMessage = 'Failed to update appointment.';
        console.error(err);
      },
    });
  }
}
