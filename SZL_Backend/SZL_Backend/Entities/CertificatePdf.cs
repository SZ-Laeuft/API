namespace SZL_Backend.Entities;

public class CertificatePdf
{
    public int ParticipateId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int RoundCount { get; set; }
    public int Place { get; set; }
}