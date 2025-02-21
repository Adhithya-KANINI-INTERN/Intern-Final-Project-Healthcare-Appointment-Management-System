import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppointmentApprovalDTO, AppointmentCancelDTO, AppointmentCompletedDTO, AppointmentCreateDTO, AppointmentDTO, AppointmentUpdateDTO } from '../shared/models/appointment.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private apiUrl = 'https://localhost:7078/api/Appointment'; // Update with actual API base URL

  constructor(private http: HttpClient) {}

  getAllAppointments(): Observable<AppointmentDTO[]> {
    return this.http.get<AppointmentDTO[]>(`${this.apiUrl}/all-appointments`);
  }

  GetTotalConfirmedAppointments(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-appointments`);
  }

  getAppointmentsbyDoctor(userId: number): Observable<AppointmentDTO[]> {
    return this.http.get<AppointmentDTO[]>(`${this.apiUrl}/by-doctor/${userId}`);
  }

  getPatientAppointments(userId: number): Observable<AppointmentDTO[]> {
    return this.http.get<AppointmentDTO[]>(`${this.apiUrl}/patient/${userId}`);
  }

  createAppointment(appointment: AppointmentCreateDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/create`, appointment, { responseType: 'text' });
  }

  updateAppointment(appointment: AppointmentUpdateDTO): Observable<string> {
    return this.http.put(`${this.apiUrl}/update/${appointment.appointmentId}`, appointment, { responseType: 'text' });
  }

  cancelAppointment(cancelDto: AppointmentCancelDTO): Observable<string> {
    return this.http.patch(`${this.apiUrl}/cancel/${cancelDto.appointmentId}`, cancelDto, { responseType: 'text' });
  }

  MarkAppointmentAsCompleted(appointmentId: number): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/completed/${appointmentId}`, appointmentId);
  }
}
