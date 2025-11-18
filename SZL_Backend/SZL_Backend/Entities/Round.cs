using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Round
{
    public int Roundsid { get; set; }

    public int? Runnerid { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Runner? Runner { get; set; }
}
