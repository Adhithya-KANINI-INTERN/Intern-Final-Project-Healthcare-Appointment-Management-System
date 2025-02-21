import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/auth.service';
import { UserService } from '../../../services/user.service';
import { User } from '../../../shared/models/user.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-users-update',
  imports: [FormsModule],
  templateUrl: './admin-users-update.component.html',
  styleUrl: './admin-users-update.component.css'
})
export class AdminUsersUpdateComponent implements OnInit {

  userProfileData : User = {
      userId: 0,
      fullName: '',
      email: '',
      role: '',
      password: '', 
    };
  
    userId: number | null = null;
  
    constructor(private route: ActivatedRoute, private userService: UserService) {}
  
    ngOnInit(): void {
      this.route.queryParams.subscribe((params) => {
        const userId = params['userId'];
        if (userId) {
          this.loadUserProfile(userId);
        }
      });
    }
  
    loadUserProfile(userId: number): void {
      this.userService.getUserById(userId).subscribe({
        next: (data) => {
          this.userProfileData = data;
        },
        error: (err) => {
          console.error(err);
        },
      });
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
}
