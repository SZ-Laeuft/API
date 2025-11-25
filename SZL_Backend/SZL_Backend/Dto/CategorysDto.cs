namespace SZL_Backend.Dto
{
    public class CategorysDto
    {
        public int Categoryid { get; init; }

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