import { Component } from '@angular/core';
import { AuthService } from '../core/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Login } from '../shared/models/user.model';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginData : Login  = {email: '', password: ''}
  showPassword = false;

  errorMessage: string | null = null;

  constructor(private authservice: AuthService, private router: Router) {}

  onLogin() {
    this.authservice.login(this.loginData).subscribe({
      next: (response)=> {
        console.log('Login successful!', response);
        this.router.navigate(['/dashboard']);
      }, 
      error:(err)=> {
        console.error('Login failed:', err);
        this.errorMessage = 'Invalid email or password.';
      },
    });
  }

  register(){
    this.router.navigate(['/register']);
  } 
  
  changepassword() {
    this.router.navigate(['/change-password']);
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}