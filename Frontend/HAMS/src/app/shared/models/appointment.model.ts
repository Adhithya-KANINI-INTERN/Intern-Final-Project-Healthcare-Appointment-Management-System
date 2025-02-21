export class AppointmentDTO {
  appointmentId!: number;
  patientId!: number;
  patientName!: number; 
  doctorId!: number;
  doctorName!: string;
  date!: string;
  startTime!: string;
  endTime!: string;
  status!: string;
  reason!: string;
}

export class AppointmentCreateDTO {
  patientId!: number;
  doctorId!: number;
  date!: string;
  startTime!: string;
  endTime!: string;
  reason!: string;
}

export class AppointmentUpdateDTO {
  appointmentId!: number;
  newDate?: string;
  newStartTime?: string;
  newEndTime?: string;
  status?: string;
  reason?: string;
}

export class AppointmentCancelDTO {
  appointmentId!: number;
  reason!: string;
}

export class AppointmentApprovalDTO {
  appointmentId!: number;
  doctorId!: number;
  isApproved!: boolean;
}
export class AppointmentCompletedDTO
{
  AppointmentId!: number;
  DoctorId!: number;
}
