import { Component, OnInit } from '@angular/core';
import { NotificationDTO } from '../../../shared/models/notification.model';
import { NotificationService } from '../../../services/notification.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-notifiaction-list',
  imports: [CommonModule],
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent implements OnInit {

  notifications: NotificationDTO[] = [];
  userId?: number;

  constructor(private notificationService: NotificationService, private authService: AuthService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    if (this.userId) {
      this.loadNotifications(this.userId);
    } else {
      console.error('User ID is undefined.');
    }
  }

  loadNotifications(userId: number): void {
    this.notificationService.getUserNotifications(userId).subscribe({
      next: (data) => {
        this.notifications = data;
      },
      error: (err) => {
        console.error('Error fetching notifications:', err);
      },
    });
  }

  markAsRead(notificationId: number): void {
    this.notificationService.markNotificationAsRead(notificationId).subscribe({
      next: () => {
        this.notifications = this.notifications.map((n) =>
          n.notificationId === notificationId ? { ...n, isRead: true } : n
        );
      },
      error: (err) => {
        console.error('Error marking notification as read:', err);
      },
    });
  }
}
