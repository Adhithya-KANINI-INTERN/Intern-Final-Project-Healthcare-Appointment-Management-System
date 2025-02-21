import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctor-user-list',
  imports: [CommonModule],
  templateUrl: './doctor-user-list.component.html',
  styleUrl: './doctor-user-list.component.css'
})
export class DoctorUserListComponent implements OnInit {

  doctors: { 
    id: number; 
    fullName: string; 
    email: string; 
    createdAt: string }[] = [];
  errorMessage: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.getDoctors();
  }

  getDoctors(): void {
    this.userService.getUsersByRole('Doctor').subscribe({
      next: (data) => {
        this.doctors = data.sort(
          (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        );
        this.errorMessage = null;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load doctors.';
      },
    });
  }


}
