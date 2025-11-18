using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Gift
{
    public int Giftid { get; set; }

    public string? Name { get; set; }

    public int? Count { get; set; }

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();
}
