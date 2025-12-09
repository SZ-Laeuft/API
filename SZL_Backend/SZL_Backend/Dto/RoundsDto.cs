namespace SZL_Backend.Dto
{
    public class RoundsDto
    {
        public int RoundId { get; set; }

        public int? ParticipateId { get; set; }

        public DateTime? RoundTimeStamp { get; set; }

    }
}

namespace SZL_Backend.Dto
{
    public class RoundsCreateDto
    {
        public int ParticipateId { get; set; }

        public DateTime? RoundTimeStamp { get; set; }
    }
}