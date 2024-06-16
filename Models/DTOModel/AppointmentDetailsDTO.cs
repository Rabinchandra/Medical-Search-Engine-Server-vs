namespace Server_.Models.DTOModel
{
    public class AppointmentDetailsDTO
    {
        public string AppointmentId { get; set; } = null!;
        public string DoctorId { get; set; } = null!;

        public string PatientId { get; set; } = null!;

        public string DoctorName { get; set; } = "";

        public string PatientName { get; set; } = "";

        public string PatientImgUrl { get; set; } = "";

        public string patientContact { get; set; } = "";

        public string status { get; set; } = "";

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

        public string Purpose { get; set; } = null!;
    }
}
