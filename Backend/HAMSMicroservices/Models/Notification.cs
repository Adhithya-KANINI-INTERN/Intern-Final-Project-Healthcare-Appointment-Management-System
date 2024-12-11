using System;
using System.Collections.Generic;

namespace HAMSMicroservices.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int? AppointmentId { get; set; }

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string NotificationType { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Appointment Appointment { get; set; }
}
