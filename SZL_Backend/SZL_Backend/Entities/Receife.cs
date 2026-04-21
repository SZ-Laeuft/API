using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class Receife
{
    public int Giftid { get; set; }

    public int Participateid { get; set; }

    public string Iscollected { get; set; } = null!;

    public virtual Gift Gift { get; set; } = null!;

    public virtual Participate Participate { get; set; } = null!;
}
