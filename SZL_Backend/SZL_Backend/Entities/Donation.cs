using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Donation
{
    public int Donationid { get; set; }

    public int? Runnerid { get; set; }

    public decimal? Amount { get; set; }

    public virtual Runner? Runner { get; set; }
}
