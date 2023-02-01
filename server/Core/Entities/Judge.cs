using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Judge
{
    [Key]
    [Required(ErrorMessage = "JudgeId is a required field!")]
    public int JudgeId { get; set; }
    
    [Required(ErrorMessage = "MembershipCardNum is a required field!")]
    public string? MembershipCardNum { get; set; }
    
    [Required(ErrorMessage = "JudgeCategory is a required field!")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public string? JudgeCategory { get; set; }
}