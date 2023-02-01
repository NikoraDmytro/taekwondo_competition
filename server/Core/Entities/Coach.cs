using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Coach
{
    [Key]
    [Required(ErrorMessage = "CoachId is a required field!")]
    public int CoachId { get; set; }
    
    [Required(ErrorMessage = "MembershipCardNum is a required field!")]
    public string? MembershipCardNum { get; set; }
    
    [Required(ErrorMessage = "InstructorCategory is a required field!")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters long!")]
    public string? InstructorCategory { get; set; }
    
    [Required(ErrorMessage = "ClubId is a required field!")]
    public int ClubId { get; set; } 
    
    [Required(ErrorMessage = "Phone is a required field!")]
    [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$",
        ErrorMessage = "Invalid phone number!")]
    public string? Phone { get; set;  }
    
    [MaxLength(254, ErrorMessage = "No more than 254 characters long!")]
    public string? Email { get; set;  }
}