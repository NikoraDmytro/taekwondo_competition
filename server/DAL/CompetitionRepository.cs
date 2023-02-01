using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace DAL;

public class CompetitionRepository: GenericRepository<Competition>, ICompetitionRepository
{
    public CompetitionRepository(string connectionString) 
        : base(connectionString) { }

    public async Task<IEnumerable<Competition>> GetCompetitions(
        CompetitionParameters parameters)
    {
        string filterQuery = GetFiltersQuery(
            new[]
            {
                "competition_name LIKE @search",
                "city = @city",
                "current_status = @status",
                "competition_level = @level",
                "start_date >= @startDateFrom",
                "start_date <= @startDateTo",
            }, 
            new[] 
            {
                parameters.Search,
                parameters.City,
                parameters.Status,
                parameters.Level,
                parameters.StartDateFrom,
                parameters.StartDateTo,
            });

        string query =
            "SELECT * FROM competitions"
            + filterQuery
            + GetSortQuery(parameters.OrderBy)
            + GetPaginationQuery(parameters.PageNumber, parameters.PageSize);

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var competitions = await ReadData(query, connection, new []
        {
            new Tuple<string, object>("@search", $"%{parameters.Search}%"),
            new Tuple<string, object>("@city", parameters.City),
            new Tuple<string, object>("@status", parameters.Status),
            new Tuple<string, object>("@level", parameters.Level),
            new Tuple<string, object>("@startDateFrom", parameters.StartDateFrom),
            new Tuple<string, object>("@startDateTo", parameters.StartDateTo),
        });

        return competitions;
    }
}