namespace SZL_Backend.Dto
{
    public class EventsDto
    {
        public int EventId { get; set; }

        public string? Name { get; set; }

        public string? Place { get; set; }

        public string? IsActive { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Categoryid { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class EventsCreateDto
    {
        public int EventId { get; set; }

        public string? Name { get; set; }

        public string? Place { get; set; }

        public string? IsActive { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? CategoryId { get; set; }
    }
}