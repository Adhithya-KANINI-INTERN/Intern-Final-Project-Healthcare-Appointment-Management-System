// import { HttpClient } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = "https://localhost:7078/api";
  private tokenKey = 'authToken';

  constructor(private http: HttpClient, private router: Router) {}

  // Login Method
  login(loginData: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/User/Login`, loginData).pipe(
      tap((response) => {
        console.log('Login Response:', response);

        const token = response.jwtTokenKey; 
        const fullName = response.fullName; 

        console.log('Storing token...');
        this.storeToken(token);

        console.log('Storing full name...');
        this.storeFullName(fullName);

        console.log('Login flow completed successfully.');
      })
    );
  }


  // Store JWT Token in Local Storage
  private storeToken(token: string): void {
    if (token) {
      try {
        localStorage.setItem(this.tokenKey, token);
        console.log('Token Stored in Local Storage:', localStorage.getItem(this.tokenKey));
      } catch (error) {
        console.error('Error storing token:', error);
      }
    } else {
      console.error('Token is undefined or null.');
    }
  }

  // Store Full Name in Local Storage
  private storeFullName(fullName: string): void {
    if (fullName) {
      try {
        localStorage.setItem('userFullName', fullName);
        console.log('Full Name Stored in Local Storage:', localStorage.getItem('userFullName'));
      } catch (error) {
        console.error('Error storing full name:', error);
      }
    } else {
      console.error('Full Name is undefined or null.');
    }
  }

 

  // Check if User is Logged In
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Retrieve JWT Token from Local Storage
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Decode JWT Token
  getDecodedToken(): any {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }
  

  getUserRole(): string {
    const decodedToken = this.getDecodedToken();
    return decodedToken?.role || '';
  }

  getUserMail(): string {
    const decodedToken = this.getDecodedToken();
    return decodedToken?.unique_name || '';
  }

  getUserId(): number {
    const decodedToken = this.getDecodedToken();
    const nameid = decodedToken?.nameid;
    return nameid ? Number(nameid) : 0;
  }

   // Retrieve Full Name from Local Storage
   getFullName(): string | null {
    const fullName = localStorage.getItem('userFullName');
    return fullName;
  }

  // Logout Method
  logout(): void {
    this.clearToken();
    this.clearFullName();
    this.router.navigate(['/login']);
  }

  // Clear JWT Token from Local Storage
  private clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  // Clear Full Name from Local Storage
  private clearFullName(): void {
    localStorage.removeItem('userFullName');
  }
}
