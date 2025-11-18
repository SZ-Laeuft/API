using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Tag
{
    public long Uid { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();
}
