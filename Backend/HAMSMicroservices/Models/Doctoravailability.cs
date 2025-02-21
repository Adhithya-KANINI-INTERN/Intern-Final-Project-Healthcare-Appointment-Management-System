using System;
using System.Collections.Generic;

namespace HAMSMicroservices.Models;

public partial class Doctoravailability
{
    public int AvailabilityId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly Date { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<Doctoravailabilityslot> Doctoravailabilityslots { get; set; } = new List<Doctoravailabilityslot>();
}
