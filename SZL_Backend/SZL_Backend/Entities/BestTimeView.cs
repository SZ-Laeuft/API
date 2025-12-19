using System;
using System.Collections.Generic;

namespace SZL_Backend.Entities;

public partial class BestTimeView
{
    public int ParticipateId { get; set; }

    public int RoundId { get; set; }

    public double BestTime { get; set; }
}
