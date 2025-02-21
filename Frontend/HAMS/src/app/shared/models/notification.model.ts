export class NotificationDTO {
    notificationId!: number;
    userId!: number;
    message!: string;
    isRead!: boolean;
    createdAt!: Date;
    notificationType!: string;
    appointmentId?: number;
  }
  
  export class CreateNotificationDTO {
    userId!: number;
    message!: string;
    notificationType!: string;
    appointmentId?: number;
  }