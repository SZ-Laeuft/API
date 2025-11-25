namespace SZL_Backend.Dto
{
    public class GiftsDto
    {
        public int Giftid { get; set; }

        public string? Name { get; set; }

        public int? Requirement { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class GiftsCreateDto
    {
    
        public string? Name { get; set; }

        public int? Requirement { get; set; }
    }
}