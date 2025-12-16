namespace SZL_Backend.Dto
{
    public class RunnersDto
    {
        public int RunnerId { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }
        
        public string? Gender { get; set; }

        public DateOnly? Birthdate { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class RunnersCreateDto
    {
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }
        
        public string? Gender { get; set; }

        public DateOnly? Birthdate { get; set; }
    }
}