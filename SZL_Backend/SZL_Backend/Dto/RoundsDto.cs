namespace SZL_Backend.Dto
{
    public class RoundsDto
    {
        public int Roundid { get; set; }

        public int? Participateid { get; set; }

        public DateTime? Roundtimestamp { get; set; }

        public double? Roundtime { get; set; }

    }
}

namespace SZL_Backend.Dto
{
    public class RoundsCreateDto
    {
        public int? Participateid { get; set; }

        public DateTime? Roundtimestamp { get; set; }
    }
}