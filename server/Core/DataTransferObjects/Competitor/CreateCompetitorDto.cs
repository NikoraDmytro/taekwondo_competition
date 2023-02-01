using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Competitor;

public class CreateCompetitorsDto
{
    [Required(ErrorMessage = "Не вказано номер членського квитка спортсмена!")]
    public IEnumerable<int> MembershipCardNums { get; set; }
    
    [Required(ErrorMessage = "Не вказано ідентифікатор змагання!")]
    public int? CompetitionId { get; set; }
}