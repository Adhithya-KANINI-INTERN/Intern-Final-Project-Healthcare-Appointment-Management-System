using System;
using System.Collections.Generic;

namespace HAMSMicroservices.Models;

public partial class Doctoravailabilityslot
{
    public int SlotId { get; set; }

    public int AvailabilityId { get; set; }

    public TimeOnly SlotStartTime { get; set; }

    public TimeOnly SlotEndTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Doctoravailability Availability { get; set; } = null!;
}
