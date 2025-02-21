import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddDoctorProfileDTO, DoctorAvailabilityDTO, DoctorDTO, DoctorSlotsDTO, VerifyDoctorDTO } from '../shared/models/doctor.model';
import { DoctorAvailabilitySlot } from '../shared/models/doctoravailabilityslot.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private apiUrl = 'https://localhost:7078/api/Doctor';

  constructor(private http: HttpClient) {}


  checkDoctorProfile(userId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/check-profile/${userId}`, )
  }

  getDoctorProfilebyId(doctorId?: number): Observable<DoctorDTO> {
    return this.http.get<DoctorDTO>(`${this.apiUrl}/profile/${doctorId}`);
  }

  getDoctorProfile(userId: number): Observable<DoctorDTO> {
    return this.http.get<DoctorDTO>(`${this.apiUrl}/profile/${userId}`);
  }

  getAllDoctorProfile() : Observable<DoctorDTO[]> {
    return this.http.get<DoctorDTO[]>(`${this.apiUrl}/all-profile`);
  }

  getDoctorsBySpecialization(specialization: string): Observable<DoctorDTO[]> {
    return this.http.get<DoctorDTO[]>(`${this.apiUrl}/specialization/${specialization}`);
  }

 
   getPendingDoctors(): Observable<DoctorDTO[]> {
    return this.http.get<DoctorDTO[]>(`${this.apiUrl}/pending`);
  }

  addDoctorProfile(addDoctorProfileDTO: AddDoctorProfileDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/add-profile`, addDoctorProfileDTO, { responseType: 'text' });
  }


  updateDoctorProfile(doctor: DoctorDTO): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/update`, doctor);
  }

  getDoctorSlots(doctorId: number, date: string): Observable<DoctorSlotsDTO[]> {
    return this.http.get<DoctorSlotsDTO[]>(`${this.apiUrl}/${doctorId}/doctor-slots?appointmentDate=${date}`);
  }

  getDoctorAvailabilitySlots(doctorId: number, date: string): Observable<DoctorAvailabilitySlot[]> {
    return this.http.get<DoctorAvailabilitySlot[]>(`${this.apiUrl}/${doctorId}/available-slots?appointmentDate=${date}`);
  }
  
  manageAvailability(availabilityData: DoctorAvailabilityDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/availability`, availabilityData, { responseType: 'text' }); 
  }
  
  
  verifyDoctorProfile(verifyDoctorDTO: VerifyDoctorDTO): Observable<string> {
    return this.http.put(`${this.apiUrl}/verify-doctor`, verifyDoctorDTO, {responseType: 'text'});
  }
}
