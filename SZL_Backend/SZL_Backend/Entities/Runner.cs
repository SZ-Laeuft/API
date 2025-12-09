using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Runner
{
    public int Runnerid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
