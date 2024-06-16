using System;
using System.Collections.Generic;

namespace Server_.Models.EntityModel;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public string UserId { get; set; } = null!;

    public string Role { get; set; } = null!;
}
