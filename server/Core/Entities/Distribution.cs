using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Distribution
{
    [Key]
    [Required(ErrorMessage = "DistributionId is a required field!")]
    public int DistributionId { get; set; }

    [Required(ErrorMessage = "DayangId is a required field!")]
    public int DayangId { get; set; }
    
    [Required(ErrorMessage = "DivisionName is a required field!")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public int DivisionName { get; set; }
    
    [Required(ErrorMessage = "SerialNum is a required field!")]
    public int SerialNum { get; set; }
    
    [Required(ErrorMessage = "JudgeRole is a required field!")]
    [RegularExpression("боковий|рефері|головний|заступник головного судді", ErrorMessage = "Invalid judge role!")]
    public string JudgeRole { get; set; } = "Regular";
}