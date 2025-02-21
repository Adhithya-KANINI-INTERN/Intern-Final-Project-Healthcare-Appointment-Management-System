import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorDTO } from '../../../shared/models/doctor.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-doctor-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-list.component.html',
  styleUrl: './doctor-list.component.css'
})
export class DoctorListComponent implements OnInit {

  doctors: DoctorDTO[] = [];
  filteredDoctors: DoctorDTO[] = [];
  errorMessage: string | null = null;
  searchQuery: string = '';

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.getAllDoctors();
  }

  getAllDoctors(): void {
    this.doctorService.getAllDoctorProfile().subscribe({
      next: (data) => {
        this.doctors = data;
        this.filteredDoctors = this.doctors;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load doctors.';
      },
    });
  }

  onSearch(): void {
    if (this.searchQuery) {
      this.filteredDoctors = this.doctors.filter(doctor =>
        doctor.doctorName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    } else {
      this.filteredDoctors = this.doctors;
    }
  }
}
