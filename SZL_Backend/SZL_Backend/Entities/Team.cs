using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Team
{
    public int Teamid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();
}
