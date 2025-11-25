namespace SZL_Backend.Dto
{
    public class DonationsDto
    {
        public int Donationid { get; set; }

        public int? Participateid { get; set; }

        public double? Amount { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class DonationsCreateDto
    {
        public int? Participateid { get; set; }

        public double? Amount { get; set; }
    }
}