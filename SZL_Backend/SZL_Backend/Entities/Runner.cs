using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Runner
{
    public int Runnerid { get; set; }

    public int? Eventid { get; set; }

    public long? Uid { get; set; }

    public int? Teamid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();

    public virtual Team? Team { get; set; }

    public virtual Tag? UidNavigation { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Gift> Gifts { get; set; } = new List<Gift>();
}
