using System;
using System.Collections.Generic;
using HAMSGateWay.Models;

namespace HAMSGateWay.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = "Confirmed";
    public string ReasonForVisit { get; set; }
    public string Cancellation { get; set; }
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual User Patient { get; set; } = null!;
}
