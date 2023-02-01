namespace Core.DataTransferObjects.Sportsman;

public class SportsmanDto
{
    public int MembershipCardNum { get; set; }
    public string? Sex { get; set; }
    public string? Belt { get; set; }
    public string? FullName { get; set; }
    public string? Photo { get; set; }
    public string? CoachFullName { get; set; }
    public string? ClubName { get; set; }
    public DateTime BirthDate { get; set; }
    public string? SportsCategory { get; set; }
}