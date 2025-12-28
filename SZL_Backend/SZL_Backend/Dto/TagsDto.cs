namespace SZL_Backend.Dto
{
    public class TagsDto
    {
        public string TagId { get; set; }

        public string? Status { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class TagsCreateDto
    {
        public string? TagId { get; set; }
        public string? Status { get; set; }
    }
}