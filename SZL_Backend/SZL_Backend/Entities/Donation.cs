using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Donation
{
    public int Donationid { get; set; }

    public int? Participateid { get; set; }

    public double? Amount { get; set; }

    public virtual Participate? Participate { get; set; }
}
