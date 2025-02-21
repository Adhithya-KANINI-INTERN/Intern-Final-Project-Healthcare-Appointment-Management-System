import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormsModule } from '@angular/forms';
import { ChangePassword } from '../../../shared/models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-password',
  imports: [FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  passwordData: ChangePassword = {
    email: '',
    newPassword: '',
  };

  reenteredPassword: string = ''; // Re-enter password field
  errorMessage: string | null = null; // Password mismatch error

  constructor(private userService: UserService) {}

  // Validate password fields
  validatePasswords(): void {
    if (
      this.passwordData.newPassword &&
      this.reenteredPassword &&
      this.passwordData.newPassword !== this.reenteredPassword
    ) {
      this.errorMessage = 'Passwords do not match.';
    } else {
      this.errorMessage = null;
    }
  }

  changePassword(): void {
    if (this.errorMessage) {
      return; 
    }

    this.userService.changePassword(this.passwordData).subscribe({
      next: () => {
        alert('Password changed successfully');
      },
      error: (err) => {
        console.error(err);
        alert('Error changing password');
      },
    });
  }
}
