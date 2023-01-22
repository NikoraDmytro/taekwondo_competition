using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Club
{
    [Required(ErrorMessage = "ClubId is a required field!")]
    public int ClubId { get; set; }
    
    [Required(ErrorMessage = "Name is a required field!")]
    public string? ClubName { get; set; }
    
    [Required(ErrorMessage = "City is a required field!")]
    public string? City { get; set; }
    
    [Required(ErrorMessage = "GymAddr is a required field!")]
    public string? GymAddr { get; set;  }
}