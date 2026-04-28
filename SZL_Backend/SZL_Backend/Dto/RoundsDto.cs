using System.ComponentModel.DataAnnotations;

namespace SZL_Backend.Dto
{
    public class RoundsDto
    {
        public int RoundId { get; init; }

        public int? ParticipateId { get; init; }

        public DateTime? RoundTimestamp { get; init; }

        public double? RoundTime { get; init; }
        
        public string? IsValid { get; init; }

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

namespace SZL_Backend.Dto
{
    public class RoundsUpdateDto(int? participateId, DateTime? roundTimestamp, string? isValid)
    {
        public int? ParticipateId { get; } = participateId;

        public DateTime? RoundTimestamp { get; } = roundTimestamp;

        [Required]
        [RegularExpression("^(true|false)$",
            ErrorMessage = "Status must be 'true' or 'false'")]
        public string? IsValid { get; set; } = isValid;

    }
}