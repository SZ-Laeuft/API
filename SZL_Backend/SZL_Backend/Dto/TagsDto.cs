using System.ComponentModel.DataAnnotations;

namespace SZL_Backend.Dto
{
    public class TagsDto
    {
        public string? TagId { get; init; }

        public string? Status { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class TagsCreateDto(string? tagId)
    {
        public string? TagId { get; } = tagId;

        [Required]
        [RegularExpression("^(taken|free)$",
            ErrorMessage = "Status must be 'taken' or 'free'")]
        public string Status { get; set; } = null!;
    }
}