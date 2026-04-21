using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Gift
{
    public int Giftid { get; set; }

    public string? Name { get; set; }

    public int? Requirement { get; set; }

    public virtual IEnumerable<Receive>? Receives { get; set; } = new List<Receive>();
}
