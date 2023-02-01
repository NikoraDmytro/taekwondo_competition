using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Sportsman
{
    [Key]
    [Required(ErrorMessage = "MembershipCardNum is a required field!")]
    public int MembershipCardNum { get; set; }
    
    [Required(ErrorMessage = "FirstName is a required field!")]
    [MaxLength(60, ErrorMessage = "No more than 60 characters long!")]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "SecondName is a required field!")]
    [MaxLength(60, ErrorMessage = "No more than 60 characters long!")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Patronymic is a required field!")]
    [MaxLength(60, ErrorMessage = "No more than 60 characters long!")]
    public string? Patronymic { get; set; }
    
    [Required(ErrorMessage = "BirthDate is a required field!")]
    public DateTime? BirthDate { get; set; }
    
    [Required(ErrorMessage = "Password is a required field!")]
    [MaxLength(100, ErrorMessage = "No more than 100 characters long!")]
    public string? Password { get; set; }
    
    [RegularExpression("Ч|Ж", ErrorMessage = "Invalid sex!")]
    public string Sex { get; set; } = "Ч";
    
    [RegularExpression("10|9|8|7|6|5|4|3|2|1|I|II|III|IV|V|VI|VII|VIII|IX", ErrorMessage = "Invalid belt!")]
    public string Belt { get; set; } = "10";
    
    [RegularExpression("Regular|Admin", ErrorMessage = "Invalid role!")]
    public string Role { get; set; } = "Regular";
    
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public string? SportsCategory { get; set; }
    
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public string? Photo { get; set; } = "placeholder.png";
    
    public int? CoachId { get; set; }
}