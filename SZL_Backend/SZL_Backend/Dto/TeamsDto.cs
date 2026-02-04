namespace SZL_Backend.Dto
{
    public class TeamsDto
    {
        public int TeamId { get; init; }

        public string? Name { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class TeamsCreateDto(string? name)
    {
        public string? Name { get; } = name;
    }
}