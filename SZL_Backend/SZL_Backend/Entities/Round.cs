using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Round
{
    public int Roundid { get; set; }

    public int? Participateid { get; set; }

    public DateTime? Roundtimestamp { get; set; }

    public double? Roundtime { get; set; }

    public virtual Participate? Participate { get; set; }
}
