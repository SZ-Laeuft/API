namespace SZL_Backend.Dto
{
    public class ParticipatesDto
    {
        public int ParticipateId { get; init; }

        public int? TeamId { get; init; }

        public string? TagId { get; init; }

        public int? RunnerId { get; init; }

        public int? EventId { get; init; }
        
        public int? CategoryId { get; init; }
        
    }
}

namespace SZL_Backend.Dto
{
    public class ParticipatesCreateDto(int? eventId, int? runnerId, string? tagId, int? teamId)
    {
        public int? TeamId { get; } = teamId;

        public string? TagId { get; } = tagId;

        public int? RunnerId { get; } = runnerId;

        public int? EventId { get; } = eventId;
    }
}