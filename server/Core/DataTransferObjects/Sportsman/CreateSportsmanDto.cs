using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Sportsman;

public class CreateSportsmanDto
{
    [Required(ErrorMessage = "Не вказано ім'я спортсмена!")]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "Не вказано прізвише спортсмена!")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Не вказано ім'я по-батькові спортсмена!")]
    public string? Patronymic { get; set; }
    
    [Required(ErrorMessage = "Не вказана дата народження спортсмена!")]
    public DateTime? BirthDate { get; set; }
    
    [Required(ErrorMessage = "Не вказан пароль від персонального акаунта спортсмена!")]
    public string? Password { get; set; }
    
    [Required(ErrorMessage = "Не вказано персонального тренера спортсмена!")]
    public int? CoachId { get; set; }
    
    public string Sex { get; set; } = "Ч";
    public string Belt { get; set; } = "10";
    public string? SportsCategory { get; set; }
    public string Role { get; set; } = "Regular";
    public string? Photo { get; set; } = "placeholder.png";
}