namespace SZL_Backend.Dto
{
    public class TagsDto
    {
        public long TagId { get; set; }

        public string? Status { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class TagsCreateDto
    {
        public long? TagId { get; set; }
        public string? Status { get; set; }
    }
}