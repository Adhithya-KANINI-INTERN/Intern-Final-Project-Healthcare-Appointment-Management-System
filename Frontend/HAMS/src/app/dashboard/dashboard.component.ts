import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { AppointmentService } from '../services/appointment.service';
import { NotificationDTO } from '../shared/models/notification.model';
import { AppointmentDTO } from '../shared/models/appointment.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { User } from '../shared/models/user.model';
import { AuthService } from '../core/auth.service';
import { DoctorService } from '../services/doctor.service';
import { DoctorDTO, VerifyDoctorDTO } from '../shared/models/doctor.model';
import { UserService } from '../services/user.service';
import { CreateDoctorProfileComponent } from "../features/doctors/create-doctor-profile/create-doctor-profile.component";
import { AvailableSlotsComponent } from "../features/doctors/available-slots/available-slots.component";

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, CreateDoctorProfileComponent, AvailableSlotsComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{

  notifications: NotificationDTO[] = [];
  appointments: AppointmentDTO[] = [];
  pendingDoctors: DoctorDTO[] = [];
  totalConfirmedAppointments?: number;
  userName?: string;
  userRole?: string;
  fullName?: string;
  userId?: number;
  totalUsers?: number;
  totalDoctors?: number;
  totalPatients?: number;
  isLoadingNotifications = true;
  isLoadingAppointments = true;
  isLoadingPendingDoctors = false;
  isLoadingConfirmedAppointments = false;
  hasDoctorProfile = false;
  isLoadingDoctorProfile = true;

  constructor(
    private notificationService: NotificationService,
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.userId = this.authService.getUserId();
    this.userName = this.authService.getFullName() || 'User';
    this.userRole = this.authService.getUserRole(); 
    if(this.userRole === 'Doctor' || this.userRole === 'Patient')
    {
      this.checkDoctorProfile(this.userId);
      // this.fetchNotifications(this.userId);
      this.fetchAppointments(this.userId);
    }

    if (this.userRole === 'Admin' || this.userRole === 'Doctor') {
      this.fetchTotalConfirmedAppointments();
    }
      
    if (this.userRole === 'Admin') {
      this.fetchPendingDoctors();
      this.fetchTotalCounts();
    }
  }

  fetchTotalCounts(): void {
    this.userService.GetTotalUsers().subscribe({
      next: (count) => (this.totalUsers = count),
      error: (err) => console.error('Error fetching total users:', err),
    });

    this.userService.GetTotalDoctors().subscribe({
      next: (count) => (this.totalDoctors = count),
      error: (err) => console.error('Error fetching total doctors:', err),
    });

    this.userService.GetTotalPatients().subscribe({
      next: (count) => (this.totalPatients = count),
      error: (err) => console.error('Error fetching total patients:', err),
    });
  }

  fetchNotifications(userId: number): void {
    this.notificationService.getUserNotifications(userId).subscribe({
      next: (data) => {
        this.notifications = data;
        this.isLoadingNotifications = false;
      },
      error: (err) => {
        console.error('Error fetching notifications:', err);
        this.isLoadingNotifications = false;
      },
    });
  }

  checkDoctorProfile(userId: number): void {
      this.doctorService.checkDoctorProfile(userId).subscribe({
        next: (exists) => {
          this.hasDoctorProfile = exists;
          this.isLoadingDoctorProfile = false;
        },
        error: (err) => {
          console.error('Error checking doctor profile:', err);
          this.isLoadingDoctorProfile = false;
        },
      });
  }

  fetchAppointments(userId: number): void {
    if (this.userRole === 'Doctor') {
      this.appointmentService.getAppointmentsbyDoctor(userId).subscribe({
        next: (data) => {
          this.appointments = data;
          this.isLoadingAppointments = false;
        },
        error: (err) => {
          console.error('Error fetching doctor appointments:', err);
          this.isLoadingAppointments = false;
        },
      });
    } else if (this.userRole === 'Patient') {
      this.appointmentService.getPatientAppointments(userId).subscribe({
        next: (data) => {
          this.appointments = data;
          this.isLoadingAppointments = false;
        },
        error: (err) => {
          console.error('Error fetching patient appointments:', err);
          this.isLoadingAppointments = false;
        },
      });
    }
  }

  fetchTotalConfirmedAppointments(): void {
    this.isLoadingConfirmedAppointments = true;
    this.appointmentService.GetTotalConfirmedAppointments().subscribe({
      next: (data) => {
        this.totalConfirmedAppointments = data;
        this.isLoadingConfirmedAppointments = false;
      },
      error: (err) => {
        console.error('Error fetching total confirmed appointments:', err);
        this.isLoadingConfirmedAppointments = false;
      },
    });
  }

  fetchPendingDoctors(): void {
    this.isLoadingPendingDoctors = true;
    this.doctorService.getPendingDoctors().subscribe({
      next: (doctors) => {
        this.pendingDoctors = doctors;
        this.isLoadingPendingDoctors = false;
      },
      error: (err) => {
        console.error('Error fetching pending doctors:', err);
        this.isLoadingPendingDoctors = false;
      },
    });
  }

  approveDoctor(doctorId: number): void {

    const verifyDoctorDTO: VerifyDoctorDTO = {
      doctorId: doctorId,
      status: 'Verified'
    };
  
    console.log('Approving doctor:', verifyDoctorDTO);
    
    this.doctorService.verifyDoctorProfile(verifyDoctorDTO).subscribe({
      next: (response) => {
        console.log('Response:', response);
        this.pendingDoctors = this.pendingDoctors.filter((doc) => doc.doctorId !== doctorId);
        alert('Doctor approved successfully.');
      },
      error: (err) => {
        console.error('Error approving doctor:', err);
        alert('Failed to approve doctor.');
      },
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

  markNotificationAsRead(notificationId: number): void {
    if (this.userId) { 
      this.notificationService.markNotificationAsRead(notificationId).subscribe({
        next: () => this.fetchNotifications(this.userId!),
        error: (err) => console.error('Error marking notification as read:', err),
      });
    } 
    else {
      console.error('User ID is undefined.');
    }
  }

  navigateToCreateAppointment(): void {
    this.router.navigate(['/create-appointment']);
  }

  navigateToManageAvailability(): void {
    this.router.navigate(['/doctor-availability']);
  }

}
