using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Club;

public class CreateClubDto
{
    [Required(ErrorMessage = "Не вказано назву клубу!")]
    public string? ClubName { get; set; }

    [Required(ErrorMessage = "Не вказано місто розташування клубу!")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Не вказано адрусу головного залу клубу!")]
    public string? GymAddr { get; set; }
}