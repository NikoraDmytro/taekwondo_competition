using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Division
{
    [Key]
    [Required(ErrorMessage = "DivisionName is a required field!")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public int DivisionName { get; set; }

    [RegularExpression("Ч|Ж", ErrorMessage = "Invalid sex!")]
    public string Sex { get; set; } = "Ч";

    [Required(ErrorMessage = "MinBelt is a required field!")]
    [RegularExpression("10|9|8|7|6|5|4|3|2|1|I|II|III|IV|V|VI|VII|VIII|IX", ErrorMessage = "Invalid belt!")]
    public string? MinBelt { get; set; }
    
    [RegularExpression("10|9|8|7|6|5|4|3|2|1|I|II|III|IV|V|VI|VII|VIII|IX", ErrorMessage = "Invalid belt!")]
    public string? MaxBelt { get; set; }
    
    [Required(ErrorMessage = "MinAge is a required field!")]
    public int MinAge { get; set; }
    
    public int MaxAge { get; set; }
    public int MinWeight { get; set; }
    public int MaxWeight { get; set; }
}