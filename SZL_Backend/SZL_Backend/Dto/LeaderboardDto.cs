namespace SZL_Backend.Dto;

public class LeaderboardByGenderDto
{
    public IEnumerable<LeaderboardEntryDto> Female { get; init; } = new List<LeaderboardEntryDto>();

    public IEnumerable<LeaderboardEntryDto> Male { get; init; } = new List<LeaderboardEntryDto>();
}

public class LeaderboardEntryDto
{
    public int ParticipateId { get; init; }

    public int? RunnerId { get; init; }

    public string? Firstname { get; init; }

    public string? Lastname { get; init; }

    public string? Gender { get; init; }

    public int RoundId { get; init; }

    public double BestTime { get; init; }
}

public class LeaderboardRoundsEntryDto
{
    public int ParticipateId { get; set; }
    public int? RunnerId { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Gender { get; set; }

    public int RoundCount { get; set; }      
    public DateTime? ReachedAt { get; set; } 
}

public class LeaderboardRoundsByGenderDto
{
    public IEnumerable<LeaderboardRoundsEntryDto> Female { get; init; } = new List<LeaderboardRoundsEntryDto>();

    public IEnumerable<LeaderboardRoundsEntryDto> Male { get; init; } = new List<LeaderboardRoundsEntryDto>();
}