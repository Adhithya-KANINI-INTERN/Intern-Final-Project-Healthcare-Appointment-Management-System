// import { Component, OnInit } from '@angular/core';
// import { DoctorService } from '../../../services/doctor.service';
// import { Router } from '@angular/router';
// import { DoctorSpecializationComponent } from '../../doctors/doctor-specialization/doctor-specialization.component';
// import { AuthService } from '../../../core/auth.service';
// import { UserService } from '../../../services/user.service';
// import { DoctorListComponent } from "../../doctors/doctor-list/doctor-list.component";
// import { DoctorProfileComponent } from "../../doctors/doctor-profile/doctor-profile.component";
// import { CommonModule } from '@angular/common';
// import { DoctorUserListComponent } from '../../doctors/doctor-user-list/doctor-user-list.component';

// @Component({
//   selector: 'app-doctor',
//   imports: [DoctorSpecializationComponent, DoctorListComponent, DoctorProfileComponent, DoctorUserListComponent, CommonModule],
//   templateUrl: './doctor.component.html',
//   styleUrl: './doctor.component.css'
// })
// export class DoctorComponent implements OnInit {
//   userRole: string | null = null;

//   constructor(private authService: AuthService, private router: Router) {}

//   ngOnInit(): void {
//     this.userRole = this.authService.getUserRole();
//   }
// }


// import { Component, OnInit } from '@angular/core';
// import { DoctorService } from '../../../services/doctor.service';
// import { DoctorDTO } from '../../../shared/models/doctor.model';
// import { CommonModule } from '@angular/common';
// import { FormsModule } from '@angular/forms';

// @Component({
//   selector: 'app-doctor',
//   imports: [CommonModule, FormsModule],
//   templateUrl: './doctor.component.html',
//   styleUrls: ['./doctor.component.css']
// })
// export class DoctorComponent implements OnInit {

//   doctors: DoctorDTO[] = [];
//   filteredDoctors: DoctorDTO[] = [];
//   specializations: string[] = [];
//   searchQuery: string = '';
//   selectedSpecialization: string = '';
//   selectedDoctor: DoctorDTO | null = null;

//   constructor(private doctorService: DoctorService) {}

//   ngOnInit(): void {
//     this.loadDoctors();
//   }

//   loadDoctors(): void {
//     this.doctorService.getAllDoctorProfile().subscribe({
//       next: (data) => {
//         this.doctors = data;
//         this.filteredDoctors = data;
//         this.specializations = [...new Set(data.map(d => d.specialization))];
//       },
//       error: (err) => console.error('Error loading doctors:', err)
//     });
//   }

//   onSearch(): void {
//     this.applyFilters();
//   }

//   onFilter(): void {
//     this.applyFilters();
//   }

//   applyFilters(): void {
//     this.filteredDoctors = this.doctors.filter(doctor =>
//       doctor.doctorName.toLowerCase().includes(this.searchQuery.toLowerCase()) &&
//       (this.selectedSpecialization === '' || doctor.specialization === this.selectedSpecialization)
//     );
//   }

//   viewDoctorDetails(doctor: DoctorDTO): void {
//     this.selectedDoctor = doctor;
//   }

//   closeDoctorDetails(): void {
//     this.selectedDoctor = null;
//   }
// }

import { Component, HostListener, OnInit } from '@angular/core';
import { DoctorService } from '../../../services/doctor.service';
import { DoctorDTO } from '../../../shared/models/doctor.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../core/auth.service';
import { DoctorProfileComponent } from "../../doctors/doctor-profile/doctor-profile.component";

@Component({
  selector: 'app-doctor',
  imports: [CommonModule, FormsModule, DoctorProfileComponent],
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.css']
})
export class DoctorComponent implements OnInit {
  userRole: string | null = null;
  doctors: DoctorDTO[] = [];
  filteredDoctors: DoctorDTO[] = [];
  specializations: string[] = [];
  searchQuery: string = '';
  selectedSpecialization: string = '';
  selectedDoctor: DoctorDTO | null = null;
  selectedDoctorUser: any | null = null;
  isUserInfoCardVisible: boolean = false;

  constructor(private doctorService: DoctorService, private userService: UserService, private authService: AuthService) {}

  ngOnInit(): void {
    this.loadDoctors();
    this.userRole = this.authService.getUserRole();
  }

  loadDoctors(): void {
    this.doctorService.getAllDoctorProfile().subscribe({
      next: (data) => {
        this.doctors = data;
        this.filteredDoctors = data;
        this.specializations = [...new Set(data.map(d => d.specialization))];
      },
      error: (err) => console.error('Error loading doctors:', err)
    });
  }

  onSearch(): void {
    this.applyFilters();
  }

  onFilter(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredDoctors = this.doctors.filter(doctor =>
      doctor.doctorName.toLowerCase().includes(this.searchQuery.toLowerCase()) &&
      (this.selectedSpecialization === '' || doctor.specialization === this.selectedSpecialization)
    );
  }

  viewDoctorUser(doctor: DoctorDTO): void {
    this.selectedDoctor = doctor;
    this.fetchDoctorUser(doctor.userId);
    this.isUserInfoCardVisible = true;
  }

  fetchDoctorUser(userId: number): void {
    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        this.selectedDoctorUser = user;
      },
      error: (err) => {
        console.error('Error fetching doctor user info:', err);
        this.selectedDoctorUser = null;
      }
    });
  }

  closeUserInfo(): void {
    this.isUserInfoCardVisible = false;
    this.selectedDoctorUser = null;
    this.selectedDoctor = null;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const targetElement = event.target as HTMLElement;
    const userInfoCard = document.querySelector('.user-info-card');
    if (userInfoCard && !userInfoCard.contains(targetElement)) {
      this.closeUserInfo();
    }
  }
}
