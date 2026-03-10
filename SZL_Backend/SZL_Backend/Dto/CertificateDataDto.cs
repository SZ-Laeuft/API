namespace SZL_Backend.Dto;

public class CertificateDataDto
{
    public int ParticipateId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int RoundCount { get; set; }
}