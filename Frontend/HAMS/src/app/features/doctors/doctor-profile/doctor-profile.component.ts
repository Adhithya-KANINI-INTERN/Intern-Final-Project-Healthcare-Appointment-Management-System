import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorDTO } from '../../../shared/models/doctor.model';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-doctor-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-profile.component.html',
  styleUrl: './doctor-profile.component.css'
})
export class DoctorProfileComponent implements OnInit {


  doctor: DoctorDTO | null = null;
  userId: number | null = null;
  isAdmin: boolean = false;
  inputDoctorId: number | null = null; // For admin to enter doctor ID

  constructor(private doctorService: DoctorService, private authService: AuthService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    const userRole = this.authService.getUserRole();
    this.isAdmin = userRole === 'Admin';

    if (!this.isAdmin && this.userId) {
      // Automatically fetch doctor profile for logged-in doctor
      this.getDoctorProfile(this.userId);
    }
  }

  // Method to fetch doctor profile
  getDoctorProfile(doctorId: number): void {
    this.doctorService.getDoctorProfile(doctorId).subscribe({
      next: (data) => (this.doctor = data),
      error: (err) => console.error('Error fetching doctor profile:', err),
    });
  }

  // For admin: Fetch profile using inputDoctorId
  fetchProfileForAdmin(): void {
    if (this.inputDoctorId) {
      this.getDoctorProfile(this.inputDoctorId);
    } else {
      console.error('Doctor ID is required.');
    }
  }

  // doctor: Doctor | null = null;
  // userId: number | null = null;

  // constructor(private doctorService: DoctorService, private authService: AuthService) {}

  // ngOnInit(): void {
  //   this.userId = this.authService.getUserId();
  //   this.doctorService.getDoctorProfile(this.userId).subscribe({
  //     next: (data) => (this.doctor = data),
  //     error: (err) => console.error('Error fetching doctor profile:', err),
  //   });
  // }
}
