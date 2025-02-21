import { Component } from '@angular/core';
import { AppointmentService } from '../../../services/appointment.service';
import { AppointmentApprovalDTO } from '../../../shared/models/appointment.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-approve-appointment',
  imports: [FormsModule, CommonModule],
  templateUrl: './approve-appointment.component.html',
  styleUrl: './approve-appointment.component.css'
})
export class ApproveAppointmentComponent {

  // approvalDto: AppointmentApprovalDTO = {
  //   appointmentId: 0,
  //   doctorId: 0,
  //   isApproved: true,
  // };
  // successMessage: string | null = null;
  // errorMessage: string | null = null;

  // constructor(private appointmentService: AppointmentService, private route: ActivatedRoute) {}

  // ngOnInit(): void {
  //   this.route.paramMap.subscribe((params) => {
  //     const appointmentId = params.get('appointmentId');
  //     if (appointmentId) {
  //       this.approvalDto.appointmentId = +appointmentId; // Convert string to number
  //     }
  //   });
  // }

  // approveAppointment(): void {
  //   this.appointmentService.approveAppointment(this.approvalDto).subscribe({
  //     next: () => {
  //       this.successMessage = `Appointment ${this.approvalDto.isApproved ? 'approved' : 'rejected'} successfully!`;
  //       this.errorMessage = null;
  //     },
  //     error: (err) => {
  //       this.errorMessage = 'Failed to process the appointment approval.';
  //       console.error(err);
  //     },
  //   });
  // }

}
