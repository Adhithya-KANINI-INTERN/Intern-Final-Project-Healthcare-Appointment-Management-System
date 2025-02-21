import { Component } from '@angular/core';
import { AuthService } from '../core/auth.service';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Register } from '../shared/models/user.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerData : Register = {
    fullName: '',
    email: '',
    password: '',
    role: '', 
  };

  reenteredPassword: string = ''; 
  errorMessage: string | null = null; 

  constructor(private userService: UserService, private router: Router) {}

  
  validatePasswords(): void {
    if (
      this.registerData.password &&
      this.reenteredPassword &&
      this.registerData.password !== this.reenteredPassword
    ) {
      this.errorMessage = 'Passwords do not match.';
    } else {
      this.errorMessage = null;
    }
  }


  register(): void {

    if (this.errorMessage) {
      return; 
    }


    this.userService.register(this.registerData).subscribe({
      next: (response) => {
        alert(response);
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error(err);
        alert('Error registering user');
      },
    });
  }
}
