namespace SZL_Backend.Dto
{
    public class GiftsDto
    {
        public int GiftId { get; init; }

        public string? Name { get; init; }

        public int? Requirement { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class GiftsCreateDto(string? name, int? requirement)
    {
        public string? Name { get; } = name;

        public int? Requirement { get; } = requirement;
    }
}