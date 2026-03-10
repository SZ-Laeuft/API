using System.ComponentModel.DataAnnotations;

namespace SZL_Backend.Dto
{
    public class EventsDto
    {
        public int EventId { get; init; }

        public string? Name { get; init; }

        public string? Place { get; init; }

        public string? IsActive { get; init; }

        public DateTime? StartTime { get; init; }

        public DateTime? EndTime { get; init; }

        public int? CategoryId { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class EventsCreateDto(string? name, string? place, DateTime? startTime, DateTime? endTime)
    {
        public int EventId { get; set; }

        public string? Name { get; } = name;

        public string? Place { get; } = place;

        [Required]
        [RegularExpression("^(true|false)$",
            ErrorMessage = "IsActive must be 'true' or 'false'")]
        public string IsActive { get; set; } = null!;

        public DateTime? StartTime { get; } = startTime;

        public DateTime? EndTime { get; } = endTime;

        public int? CategoryId { get; set; }
    }
}