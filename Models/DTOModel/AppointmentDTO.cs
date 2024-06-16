namespace Server_.Models.DTOModel
{
    public class AppointmentDTO
    {
        public string DoctorId { get; set; } = null!;

        public string PatientId { get; set; } = null!;

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

        public string Purpose { get; set; } = null!;
    }
}
