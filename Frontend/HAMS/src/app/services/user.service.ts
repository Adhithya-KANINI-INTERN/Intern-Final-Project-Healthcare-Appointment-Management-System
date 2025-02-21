import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ChangePassword, Login, Register, User } from '../shared/models/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7078/api/User'; // Replace with your backend URL

  constructor(private http: HttpClient) {}


  GetTotalUsers(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-users`);
  }

  GetTotalDoctors(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-doctors`);
  }

  GetTotalPatients(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-patients`);
  }

  getAllUsers(): Observable<any[]> {
    return this.http.get<User[]>(`${this.apiUrl}`);
  }

  
  getUserById(userId: number): Observable<any> {
    return this.http.get<User>(`${this.apiUrl}/userId?userId=${userId}`);
  }

  
  getUsersByRole(role: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/role/${role}`);
  }

  
  login(loginData: Login): Observable<any> {
    return this.http.post<Login>(`${this.apiUrl}/Login`, loginData);
  }

  register(registerData: Register): Observable<string> {
    return this.http.post(`${this.apiUrl}/Register`, registerData, { responseType: 'text' });
  }


  updateUser(userProfileData: User): Observable<string> {
    return this.http.put(`${this.apiUrl}`, userProfileData, { responseType: 'text' });
  }


  deleteUser(email: string): Observable<string> {
    return this.http.delete(`${this.apiUrl}/${email}/delete`, { responseType: 'text' });
  }

  changePassword(changePasswordData: ChangePassword): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/change-password`, changePasswordData);
  }
}