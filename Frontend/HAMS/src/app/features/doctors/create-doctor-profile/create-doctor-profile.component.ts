import { Component, OnInit } from '@angular/core';
import { AddDoctorProfileDTO } from '../../../shared/models/doctor.model';
import { DoctorService } from '../../../services/doctor.service';
import { AuthService } from '../../../core/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-doctor-profile',
  imports: [FormsModule, CommonModule],
  templateUrl: './create-doctor-profile.component.html',
  styleUrls: ['./create-doctor-profile.component.css']
})
export class CreateDoctorProfileComponent implements OnInit {
  doctor: AddDoctorProfileDTO = {
    userId: 0,
    specialization: '',
    yearsOfExperience: 0 
  };

  formSubmitted: boolean = false;
  errorMessage: string | null = null;

  constructor(private doctorService: DoctorService, private authService: AuthService) {}

  ngOnInit(): void {
    this.doctor.userId = this.authService.getUserId();
  }

  addDoctorProfile(): void {
    this.formSubmitted = true; 

    if (this.doctor.specialization && this.doctor.yearsOfExperience) {
      this.doctorService.addDoctorProfile(this.doctor).subscribe({
        next: (response) => {
          if (response) {
            alert(response);
          } else {
            alert('Failed to create doctor profile.');
          }
        },
        error: (err) => {
          console.error(err);
          alert('An error occurred. Please try again later.');
        }
      });
    }
  }
}
