using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Competition;

public class CreateCompetitionDto
{
    [Required(ErrorMessage = "Не вказано назву змагання!")]
    [MaxLength(100, ErrorMessage = "Не більше 100 символів у довжину!")]
    public string? CompetitionName { get; set; }
    
    [Required(ErrorMessage = "Не вказано дату зважування!")]
    public DateTime? WeightingDate { get; set; }
    
    [Required(ErrorMessage = "Не вказано дату початку змагання!")]
    public DateTime? StartDate { get; set; }
    
    [Required(ErrorMessage = "Не вказано дату закінчення змагання!")]
    public DateTime? EndDate { get; set; }
    
    [Required(ErrorMessage = "Не вказано місто, де проходитиме змагання!")]
    [MaxLength(30, ErrorMessage = "Не більше 30 символів у довжину!")]
    public string? City { get; set; }

    [MaxLength(50, ErrorMessage = "Не більше 50 символів у довжину!")]
    public string CompetitionLevel { get; set; } = "Інше";
}