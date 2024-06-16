using System;
using System.Collections.Generic;

namespace Server_.Models.EntityModel;

public partial class Doctor
{
    public string DoctorId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Speciality { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Availability { get; set; }

    public string? ProfileImgUrl { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
