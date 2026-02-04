namespace SZL_Backend.Dto
{
    public class RunnersDto
    {
        public int RunnerId { get; init; }

        public string? Firstname { get; init; }

        public string? Lastname { get; init; }
        
        public string? Gender { get; init; }

        public DateOnly? Birthdate { get; init; }
    }
}

namespace SZL_Backend.Dto
{
    public class RunnersCreateDto(DateOnly? birthdate, string? gender, string? lastname, string? firstname)
    {
        public string? Firstname { get; } = firstname;

        public string? Lastname { get; } = lastname;

        public string? Gender { get; } = gender;

        public DateOnly? Birthdate { get; } = birthdate;
    }
}