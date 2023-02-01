using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Club
{
    [Key]
    [Required(ErrorMessage = "ClubId is a required field!")]
    public int ClubId { get; set; }
    
    [Required(ErrorMessage = "Name is a required field!")]
    [MaxLength(120, ErrorMessage = "No more than 120 characters long!")]
    public string? ClubName { get; set; }
    
    [Required(ErrorMessage = "City is a required field!")]
    [MaxLength(100, ErrorMessage = "No more than 100 characters long!")]
    public string? City { get; set; }
    
    [Required(ErrorMessage = "GymAddr is a required field!")]
    [MaxLength(200, ErrorMessage = "No more than 200 characters long!")]
    public string? GymAddr { get; set;  }
}