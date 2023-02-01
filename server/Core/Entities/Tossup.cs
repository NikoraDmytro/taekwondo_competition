using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Tossup
{
    [Key]
    [Required(ErrorMessage = "TossupId is a required field!")]
    public int TossupId { get; set; }

    [Required(ErrorMessage = "CompetitorInBlue is a required field!")]
    public int CompetitorInBlue { get; set; }
    
    public int CompetitorInRed { get; set; }
    
    [Required(ErrorMessage = "LapNum is a required field!")]
    public int LapNum { get; set; }
    
    [Required(ErrorMessage = "SerialNum is a required field!")]
    public int PairSerialNum { get; set; }
}