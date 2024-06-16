using System;
using System.Collections.Generic;

namespace Server_.Models.EntityModel;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public string PatientId { get; set; } = null!;

    public string DoctorId { get; set; } = null!;

    public DateOnly RecordDate { get; set; }

    public string? Description { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
