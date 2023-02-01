namespace Core.RequestFeatures;

public class PagingParameters:RequestParameters
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}