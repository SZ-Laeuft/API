using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Event
{
    public int Eventid { get; set; }

    public string? Name { get; set; }

    public string? Place { get; set; }

    public string? Isactive { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
