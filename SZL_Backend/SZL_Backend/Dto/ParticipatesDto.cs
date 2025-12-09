namespace SZL_Backend.Dto
{
    public class ParticipatesDto
    {
        public int ParticipateId { get; set; }

        public int? TeamId { get; set; }

        public int? TagId { get; set; }

        public int? RunnerId { get; set; }

        public int? EventId { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class ParticipatesCreateDto
    {
        public int? TeamId { get; set; }

        public int? TagId { get; set; }

        public int? RunnerId { get; set; }

        public int? EventId { get; set; }
    }
}