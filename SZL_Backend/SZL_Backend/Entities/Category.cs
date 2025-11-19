using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Category
{
    public int Categoryid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
