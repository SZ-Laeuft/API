using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Category
{
    public int Categoryid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
