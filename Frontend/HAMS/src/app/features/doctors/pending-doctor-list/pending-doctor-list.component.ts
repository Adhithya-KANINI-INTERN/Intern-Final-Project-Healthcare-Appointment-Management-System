import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../../services/doctor.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DoctorDTO, VerifyDoctorDTO } from '../../../shared/models/doctor.model';

@Component({
  selector: 'app-pending-doctor-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './pending-doctor-list.component.html',
  styleUrl: './pending-doctor-list.component.css'
})
export class PendingDoctorListComponent implements OnInit {
  pendingDoctors: DoctorDTO[] = [];
  loading: boolean = false;
  errorMessage: string = '';

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.fetchPendingDoctors();
  }

  fetchPendingDoctors(): void {
    this.loading = true;
    this.errorMessage = '';
    this.doctorService.getPendingDoctors().subscribe({
      next: (doctors) => {
        this.pendingDoctors = doctors;
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to fetch pending doctors.';
        this.loading = false;
        console.error(error);
      }
    });
  }

  approveDoctor(doctorId: number): void {
    const verifyDoctorDTO: VerifyDoctorDTO = {
      doctorId,
      status: 'Verified',
    };

    this.doctorService.verifyDoctorProfile(verifyDoctorDTO).subscribe({
      next: () => {
        this.pendingDoctors = this.pendingDoctors.filter(
          (doctor) => doctor.doctorId !== doctorId
        );
        alert('Doctor approved successfully.');
      },
      error: (error) => {
        alert('Failed to approve doctor.');
        console.error(error);
      }
    });
  }

  rejectDoctor(doctorId: number): void {
    const verifyDoctorDTO: VerifyDoctorDTO = {
      doctorId,
      status: 'Rejected',
    };

    this.doctorService.verifyDoctorProfile(verifyDoctorDTO).subscribe({
      next: () => {
        this.pendingDoctors = this.pendingDoctors.filter(
          (doctor) => doctor.doctorId !== doctorId
        );
        alert('Doctor rejected successfully.');
      },
      error: (error) => {
        alert('Failed to reject doctor.');
        console.error(error);
      }
    });
  }
}
