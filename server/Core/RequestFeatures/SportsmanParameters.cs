namespace Core.RequestFeatures;

public class SportsmanParameters: PagingParameters
{
    public string Club { get; set; } = String.Empty;
    public string Coach { get; set; } = String.Empty;
    public string Sex { get; set; } = String.Empty;
    public string Belt { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
    public string SportsCategory { get; set; } = String.Empty;
    
    public int MinAge { get; set; } = 0;
    public int MaxAge { get; set; } = 100;
}