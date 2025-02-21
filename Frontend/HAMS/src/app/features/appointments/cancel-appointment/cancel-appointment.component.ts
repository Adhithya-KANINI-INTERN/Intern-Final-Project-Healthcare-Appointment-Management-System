import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../services/appointment.service';
import { AppointmentCancelDTO } from '../../../shared/models/appointment.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-cancel-appointment',
  imports: [FormsModule, CommonModule],
  templateUrl: './cancel-appointment.component.html',
  styleUrls: ['./cancel-appointment.component.css']
})
export class CancelAppointmentComponent implements OnInit {
  cancelDto: AppointmentCancelDTO = {
    appointmentId: 0,
    reason: '',
  };
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(private appointmentService: AppointmentService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const appointmentId = params.get('appointmentId');
      if (appointmentId) {
        this.cancelDto.appointmentId = +appointmentId;
      }
    });
  }

  cancelAppointment(): void {
    this.appointmentService.cancelAppointment(this.cancelDto).subscribe({
      next: (response: string) => {
        this.successMessage = response;
        this.errorMessage = null;
      },
      error: (err) => {
        this.errorMessage = 'Failed to cancel appointment.';
        console.error(err);
      },
    });
  }
}
