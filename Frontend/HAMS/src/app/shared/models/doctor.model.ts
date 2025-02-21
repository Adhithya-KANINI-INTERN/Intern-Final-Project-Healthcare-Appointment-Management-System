export class DoctorDTO {
  doctorId!: number;
  doctorName!: string;
  specialization!: string;
  userId!: number;
  yearsOfExperience!: number;
  verificationStatus!: string;
}

export class VerifyDoctorDTO {
  doctorId!: number;
  status!: string;
}

export class AddDoctorProfileDTO {
  userId!: number;
  specialization!: string;
  yearsOfExperience! : number; 
}

export class DoctorAvailabilityDTO {
  doctorId!: number;
  date!: string; 
  dayOfWeek!: string;
  startTime!: string; 
  endTime!: string; 
  slotDurationMinutes!: number;
  userId!: number;
}


export class DoctorSlotsDTO {
  slotStartTime!: string; 
  slotEndTime!: string;
  isAvailable!: boolean; 
}
