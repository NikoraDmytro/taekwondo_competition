namespace Core.RequestFeatures;

public class CompetitorParameters: PagingParameters
{
    public string Sex { get; set; } = String.Empty;
    public string Belt { get; set; } = String.Empty;
    public string Club { get; set; } = String.Empty;
    public string Coach { get; set; } = String.Empty;
    public string Division { get; set; } = String.Empty;
    
    public string MinAge { get; set; } = "0";
    public string MaxAge { get; set; } = "100";
}