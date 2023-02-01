using System.ComponentModel.DataAnnotations;

namespace Core.RequestFeatures;

public class CompetitionParameters: PagingParameters
{
    public string City { get; set; } = String.Empty;
    public string Status { get; set; } = String.Empty;
    public string Level { get; set; } = String.Empty;
    
    [RegularExpression(
        @"/([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))/", 
        ErrorMessage = "Формат дати має бути РРРР-ММ-ДД!")]
    public string StartDateFrom { get; set; } = String.Empty;
    
    [RegularExpression(
        @"/([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))/", 
        ErrorMessage = "Формат дати має бути РРРР-ММ-ДД!")]
    public string StartDateTo { get; set; } = String.Empty;
}