using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Competitor
{
    [Key]
    [Required(ErrorMessage = "ApplicationNum is a required field!")]
    public int ApplicationNum { get; set; }
    
    [Required(ErrorMessage = "MembershipCardNum is a required field!")]
    public int MembershipCardNum { get; set; }
    
    [Required(ErrorMessage = "CompetitionId is a required field!")]
    public int CompetitionId { get; set; }
    
    [Required(ErrorMessage = "Belt is a required field!")]
    [RegularExpression("10|9|8|7|6|5|4|3|2|1|I|II|III|IV|V|VI|VII|VIII|IX", ErrorMessage = "Invalid belt!")]
    public string? Belt { get; set; }
    
    public int WeightingResult { get; set; }
}