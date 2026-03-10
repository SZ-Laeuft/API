namespace SZL_Backend.Dto
{
    public class RoundsDto
    {
        public int RoundId { get; init; }

        public int? ParticipateId { get; init; }

        public DateTime? RoundTimestamp { get; init; }

        public double? RoundTime { get; init; }

    }
}

namespace SZL_Backend.Dto
{
    public class RoundsCreateDto(int? participateId, DateTime? roundTimestamp)
    {
        public int? ParticipateId { get; } = participateId;

        public DateTime? RoundTimestamp { get; } = roundTimestamp;
    }
}