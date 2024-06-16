using System;
using System.Collections.Generic;

namespace Server_.Models.EntityModel;

public partial class Admin
{
    public string AdminId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ProfileImgUrl { get; set; }
}
