namespace SZL_Backend.Dto
{
    public class DonationsDto
    {
        public int DonationId { get; init; }

        public int? ParticipateId { get; init; }

        public double? Amount { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class DonationsCreateDto(int? participateId, double? amount)
    {
        public int? ParticipateId { get; } = participateId;

        public double? Amount { get; } = amount;
    }
}