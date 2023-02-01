using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class JudgingStaff
{
    [Key]
    [Required(ErrorMessage = "ApplicationNum is a required field!")]
    public int ApplicationNum { get; set; }

    [Required(ErrorMessage = "DayangId is a required field!")]
    public int DayangId { get; set; }
    
    [Required(ErrorMessage = "JudgeId is a required field!")]
    public int JudgeId { get; set; }
    
    [Required(ErrorMessage = "JudgeRole is a required field!")]
    [RegularExpression("боковий|рефері|головний|заступник головного судді", ErrorMessage = "Invalid judge role!")]
    public string JudgeRole { get; set; } = "Regular";
}