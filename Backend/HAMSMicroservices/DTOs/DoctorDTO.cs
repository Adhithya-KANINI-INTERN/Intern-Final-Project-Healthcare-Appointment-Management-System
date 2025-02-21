namespace HAMSMicroservices.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Token { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public int YearsOfExperience { get; set; }

    }

    public class SpecializationDTO
    {
        public string Specialization { get; set; }
        public string Token { get; set; }
    }
    public class AddDoctorProfileDTO
    {
        public int UserId { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
    }

    public class VerifyDoctorDTO
    {
        public int DoctorId { get; set; }
        public string Status { get; set; }
    }


}
