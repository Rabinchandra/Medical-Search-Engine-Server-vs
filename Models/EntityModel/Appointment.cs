using System;
using System.Collections.Generic;

namespace Server_.Models.EntityModel;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public string DoctorId { get; set; } = null!;

    public string PatientId { get; set; } = null!;

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? AppointmentTime { get; set; }

    public string? Status { get; set; }

    public string Purpose { get; set; } = null!;

    public string? Notes { get; set; }

    public string? MeetingUrl { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
