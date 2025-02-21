import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateNotificationDTO, NotificationDTO } from '../shared/models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private baseUrl = 'https://localhost:7078/api/Notification'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  getUserNotifications(userId: number): Observable<NotificationDTO[]> {
    return this.http.get<NotificationDTO[]>(`${this.baseUrl}/user-notifications/${userId}`);
  }

  getUpcomingReminders(userId: number): Observable<NotificationDTO[]> {
    return this.http.get<NotificationDTO[]>(`${this.baseUrl}/reminders/${userId}`);
  }

  markNotificationAsRead(notificationId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/mark-as-read/${notificationId}`, {});
  }
}
