namespace SZL_Backend.Dto
{
    public class TagsDto
    {
        public int TagId { get; set; }

        public string? Status { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class TagsCreateDto
    {
        public int? TagId { get; set; }
        public string? Status { get; set; }
    }
}