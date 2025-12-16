using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Gift
{
    public int Giftid { get; set; }

    public string? Name { get; set; }

    public int? Requirement { get; set; }

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
