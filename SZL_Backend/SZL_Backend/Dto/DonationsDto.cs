namespace SZL_Backend.Dto
{
    public class DonationsDto
    {
        public int DonationId { get; set; }

        public int? ParticipateId { get; set; }

        public double? Amount { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class DonationsCreateDto
    {
        public int? ParticipateId { get; set; }

        public double? Amount { get; set; }
    }
}