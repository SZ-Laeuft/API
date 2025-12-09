namespace SZL_Backend.Dto
{
    public class CategoriesDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;
    }
}

namespace SZL_Backend.Dto
{
    public class CategoriesCreateDto
    {

        public string Name { get; set; } = null!;
    }
}