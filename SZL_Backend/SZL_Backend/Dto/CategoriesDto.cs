namespace SZL_Backend.Dto
{
    public class CategoriesDto
    {
        public int CategoryId { get; init; }

        public string Name { get; init; } = null!;
    }
}

namespace SZL_Backend.Dto
{
    public class CategoriesCreateDto
    {

        public string Name { get; set; } = null!;
    }
}