using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Tag
{
    public int Tagid { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
