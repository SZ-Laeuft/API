using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Participate
{
    public int Participateid { get; set; }

    public int? Teamid { get; set; }

    public int? Runnerid { get; set; }

    public int? Eventid { get; set; }

    public long? Tagid { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual Event? Event { get; set; }

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();

    public virtual Runner? Runner { get; set; }

    public virtual Tag? Tag { get; set; }

    public virtual Team? Team { get; set; }

    public virtual ICollection<Gift> Gifts { get; set; } = new List<Gift>();
}
