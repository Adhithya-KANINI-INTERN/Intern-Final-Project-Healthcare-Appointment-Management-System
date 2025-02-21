import { Component } from '@angular/core';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorDTO } from '../../../shared/models/doctor.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-doctor-specialization',
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-specialization.component.html',
  styleUrl: './doctor-specialization.component.css'
})
export class DoctorSpecializationComponent {

  doctors: DoctorDTO[] = [];
  specialization = '';

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.fetchDoctors(''); // Show all doctors by default
  }

  fetchDoctors(specialization: string): void {
    if (specialization) {
      this.doctorService.getDoctorsBySpecialization(specialization).subscribe({
        next: (data) => (this.doctors = data),
        error: (err) => console.error('Error fetching doctors:', err),
      });
    } else {
      this.doctorService.getAllDoctorProfile().subscribe({
        next: (data) => (this.doctors = data),
        error: (err) => console.error('Error fetching doctors:', err),
      });
    }
  }
}
