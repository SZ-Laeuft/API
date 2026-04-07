using System.ComponentModel.DataAnnotations;

namespace SZL_Backend.Dto;

public class ReceivesDto
{
    public int GiftId { get; set; }
    
    public int ParticipateId { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [RegularExpression("^(true|false)$",
        ErrorMessage = "Status must be 'true' or 'false'")]
    public string IsCollected { get; set; } = null!;
}