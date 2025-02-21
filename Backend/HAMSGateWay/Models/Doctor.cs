using HAMSGateWay.Models;
using System;
using System.Collections.Generic;

namespace HAMSGateWay.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public int UserId { get; set; }

    public string Specialization { get; set; } = null!;
    public int YearsOfExperience { get; set; } 
    public string VerificationStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Doctoravailability> Doctoravailabilities { get; set; } = new List<Doctoravailability>();

    public virtual User User { get; set; } = null!;
}
