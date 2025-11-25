namespace SZL_Backend.Dto
{
    public class CategoriesDto
    {
        public int Categoryid { get; set; }

        public string Name { get; set; } = null!;
    }
}

namespace SZL_Backend.Dto
{
    public class CategorysCreateDto
    {

        public string Name { get; set; } = null!;
    }
}