namespace Core.DataTransferObjects.Competitor;

public class CompetitorDto
{
    public int ApplicationNum { get; set; }
    public int MembershipCardNum { get; set; }
    public string FullName { get; set; } = String.Empty;
    public string Belt { get; set; } = String.Empty;
    public string Sex { get; set; }
    public long Age { get; set; } 
    public DateTime BirthDate { get; set; } 
    public string ClubName { get; set; } = String.Empty;
    public string CoachFullName { get; set; } = String.Empty;
    public int WeightingResult { get; set; }
    public string DivisionName { get; set; } = String.Empty;
    public string SportsCategory { get; set; } = String.Empty;
}