using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Dayang
{
    [Key]
    [Required(ErrorMessage = "DayangId is a required field!")]
    public int DayangId { get; set; }
    
    [Required(ErrorMessage = "CompetitionId is a required field!")]
    public int CompetitionId { get; set; }
}