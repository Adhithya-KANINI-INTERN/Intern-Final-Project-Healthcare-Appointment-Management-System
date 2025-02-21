import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { User } from '../../../shared/models/user.model';
import { AuthService } from '../../../core/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  imports: [FormsModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {

  userProfileData : User = {
    userId: 0,
    fullName: '',
    email: '',
    role: '',
    password: '', 
  };

  userId: number | null = null;

  constructor(private userService: UserService, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    if (this.userId) {
      this.userService.getUserById(this.userId).subscribe({
        next: (data) => {
          this.userProfileData = data;
        },
        error: (err) => {
          console.error(err);
        },
      });
    } else {
      console.error('Email is null. Cannot load user profile.');
    }
  }
  

  updateProfile(): void {
    this.userService.updateUser(this.userProfileData).subscribe({
      next: () => {
        alert('Profile updated successfully');
      },
      error: (err) => {
        console.error(err);
        alert('Error updating profile');
      },
    });
  }

  changepassword() {
    this.router.navigate(['/change-password']);
  }
}
