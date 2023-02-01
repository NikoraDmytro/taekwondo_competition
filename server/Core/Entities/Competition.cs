using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Competition
{
    [Key]
    [Required(ErrorMessage = "CompetitionId is a required field!")]
    public int CompetitionId { get; set; }
    
    [Required(ErrorMessage = "CompetitionName is a required field!")]
    [MaxLength(100, ErrorMessage = "No more than 100 characters long!")]
    public string? CompetitionName { get; set; }
    
    [Required(ErrorMessage = "WeightingDate is a required field!")]
    public DateTime WeightingDate { get; set; }
    
    [Required(ErrorMessage = "StartDate is a required field!")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "EndDate is a required field!")]
    public DateTime EndDate { get; set; }
    
    [Required(ErrorMessage = "City is a required field!")]
    [MaxLength(30, ErrorMessage = "No more than 30 characters long!")]
    public string? City { get; set; }

    [RegularExpression("очікується|прийом заявок|прийом заявок закінчено|в процесі|закінчено",
        ErrorMessage = "Invalid current status!")]
    public string CurrentStatus { get; set; } = "очікується";
    
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public string CompetitionLevel { get; set; } = "Інші турніри";
}