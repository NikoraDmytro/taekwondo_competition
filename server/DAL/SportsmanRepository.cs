using Core.DataTransferObjects.Sportsman;
using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace DAL;

public class SportsmanRepository : GenericRepository<Sportsman>, ISportsmanRepository
{
    public SportsmanRepository(string connectionString)
        : base(connectionString) { }

    private const string SelectString = @"
        SELECT s.membership_card_num,
               CONCAT_WS(' ', s.first_name, s.last_name, s.patronymic) as full_name,
               s.sex,
               s.birth_date,
               s.belt,
               cl.club_name,
               CONCAT_WS(' ', s2.first_name, s2.last_name, s2.patronymic) as coach_full_name,
               s.photo,
               s.sports_category
               FROM sportsmans s
            INNER JOIN coaches c
                ON s.coach_id = c.coach_id
            INNER JOIN sportsmans s2
                ON c.membership_card_num = s2.membership_card_num
            INNER JOIN clubs cl
                ON cl.club_id = c.club_id";

    public async Task<(IEnumerable<SportsmanDto>, int)> GetSportsmans(SportsmanParameters parameters)
    {
        string filterQuery = GetFiltersQuery(
            new[]
            {
                "CONCAT_WS(' ', s.first_name, s.last_name, s.patronymic) " +
                "LIKE @search",
                "s.belt = @belt",
                "s.role = @role",
                "s.sex = @sex",
                "s.sports_category = @sports_category",
                "c.club_id = @clubId",
                "s.coach_id = @coachId",
                "TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) " +
                "BETWEEN @minAge AND @maxAge"
            },
            new[]
            {
                parameters.Search,
                parameters.Belt,
                parameters.Role,
                parameters.Sex,
                parameters.SportsCategory,
                parameters.Club,
                parameters.Coach,
                parameters.MaxAge.ToString(),
            });
        var sqlParams = new[]
        {
            new Tuple<string, object>("@search", $"%{parameters.Search}%"),
            new Tuple<string, object>("@sex", parameters.Sex),
            new Tuple<string, object>("@belt", parameters.Belt),
            new Tuple<string, object>("@role", parameters.Role),
            new Tuple<string, object>("@coachId", parameters.Coach),
            new Tuple<string, object>("@club_id", parameters.Club),
            new Tuple<string, object>("@minAge", parameters.MinAge),
            new Tuple<string, object>("@maxAge", parameters.MaxAge),
            new Tuple<string, object>("@sports_category", parameters.SportsCategory),
        };


        string query = SelectString
                       + filterQuery
                       + GetSortQuery(parameters.OrderBy)
                       + GetPaginationQuery(parameters.PageNumber, parameters.PageSize);

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var sportsmans = await ReadData<SportsmanDto>(query, connection, sqlParams);
        var count = await Count(SelectString.Split("FROM")[1], connection, sqlParams);

        return (sportsmans, count);
    }

    public async Task<SportsmanDto?> GetSportsman(object key)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        string query = SelectString +
                       $" WHERE application_num = {key}";

        var sportsman = await ReadSingle<SportsmanDto>(query, connection);

        return sportsman;
    }

    public async Task<(IEnumerable<SportsmanDto>, int)> GetSportsmansForCompetition(int competitionId, SportsmanParameters parameters)
    {
        string filterQuery = GetFiltersQuery(
            new[]
            {
                "CONCAT_WS(' ', s.first_name, s.last_name, s.patronymic) " +
                "LIKE @search",
                "s.belt = @belt",
                "s.role != 'Admin'",
                "s.sex = @sex",
                "s.sports_category = @sports_category",
                "c.club_id = @clubId",
                "s.coach_id = @coachId",
                "TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) " +
                "BETWEEN @minAge AND @maxAge",
                @"s.membership_card_num NOT IN 
                    (SELECT membership_card_num FROM competitors
                     WHERE competition_id = @competitionId)"
            },
            new[]
            {
                parameters.Search,
                parameters.Belt,
                "true",
                parameters.Sex,
                parameters.SportsCategory,
                parameters.Club,
                parameters.Coach,
                parameters.MaxAge.ToString(),
                "true"
            });
        var sqlParams = new[]
        {
            new Tuple<string, object>("@search", $"%{parameters.Search}%"),
            new Tuple<string, object>("@sex", parameters.Sex),
            new Tuple<string, object>("@belt", parameters.Belt),
            new Tuple<string, object>("@coachId", parameters.Coach),
            new Tuple<string, object>("@club_id", parameters.Club),
            new Tuple<string, object>("@minAge", parameters.MinAge),
            new Tuple<string, object>("@maxAge", parameters.MaxAge),
            new Tuple<string, object>("@sports_category", parameters.SportsCategory),
            new Tuple<string, object>("@competitionId", competitionId)
        };


        string query = SelectString
                       + filterQuery
                       + GetSortQuery(parameters.OrderBy)
                       + GetPaginationQuery(parameters.PageNumber, parameters.PageSize);

        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var sportsmans = await ReadData<SportsmanDto>(query, connection, sqlParams);
        var count = await Count(SelectString.Split("FROM")[1], connection, sqlParams);

        return (sportsmans, count);
    }
}