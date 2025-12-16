using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Besttime
{
    public int Besttimeid { get; set; }

    public double? Besttime1 { get; set; }

    public int? ParticipateId { get; set; }

    public virtual Participate? Participate { get; set; }
}
