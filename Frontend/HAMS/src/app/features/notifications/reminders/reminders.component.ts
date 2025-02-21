import { Component, OnInit } from '@angular/core';
import { NotificationDTO } from '../../../shared/models/notification.model';
import { NotificationService } from '../../../services/notification.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-reminders',
  imports: [CommonModule],
  templateUrl: './reminders.component.html',
  styleUrl: './reminders.component.css'
})
export class RemindersComponent implements OnInit {

  reminders: NotificationDTO[] = [];
  userId?: number; 
  errorMessage?: string; 

  constructor(private notificationService: NotificationService, private authService: AuthService) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId(); 
    if (this.userId) {
      this.loadReminders(this.userId);
    } else {
      console.error('User ID is undefined.');
      this.errorMessage = 'User ID is undefined. Please log in again.';
    }
  }

  loadReminders(userId: number): void {
    this.notificationService.getUpcomingReminders(userId).subscribe({
      next: (data) => {
        if (data.length > 0) {
          this.reminders = data;
          // this.errorMessage = undefined;
        } else {
          this.errorMessage = 'No upcoming reminders found.';
        }
      },
      error: (err) => {
        console.error('Error fetching reminders:', err);
        if (err.status === 404) {
          this.errorMessage = err.error || 'No upcoming reminders found.';
        } else {
          this.errorMessage = 'An unexpected error occurred while fetching reminders.';
        }
      },
    });
  }
}
