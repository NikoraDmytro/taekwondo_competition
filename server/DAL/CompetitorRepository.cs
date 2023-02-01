using System.Text;
using Core.DataTransferObjects.Competitor;
using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;
using MySql.Data.MySqlClient;

namespace DAL;

public class CompetitorRepository: GenericRepository<Competitor>, ICompetitorRepository
{
    public CompetitorRepository(string connectionString) 
        : base(connectionString) { }

    private const string SelectString = @"
        SELECT cp.application_num, 
               s.membership_card_num,
               CONCAT_WS(' ', s.first_name, s.last_name, s.patronymic) as full_name,
               s.sex,
               TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) as age,
               s.birth_date,
               cp.belt,
               cl.club_name,
               CONCAT_WS(' ', s2.first_name, s2.last_name, s2.patronymic) as coach_full_name,
               cp.weighting_result,
               d.division_name,
               s.sports_category
               FROM competitors cp
        INNER JOIN sportsmans s
            ON cp.membership_card_num = s.membership_card_num
        INNER JOIN coaches c
            ON s.coach_id = c.coach_id
        INNER JOIN sportsmans s2
            ON c.membership_card_num = s2.membership_card_num
        INNER JOIN clubs cl
            ON cl.club_id = c.club_id
        LEFT JOIN divisions d
            ON TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) >= d.min_age AND 
               (TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) <= d.max_age OR 
                d.max_age IS NULL) AND
               s.sex = d.sex AND
               (cp.weighting_result >= d.min_weight OR 
                d.min_weight IS NULL) AND
               (cp.weighting_result < d.max_weight OR 
                d.max_weight IS NULL) AND
               (cp.belt >= d.min_belt OR 
                d.min_belt IS NULL) AND
               (cp.belt <= d.max_belt OR 
                d.max_belt IS NULL)";
    
    public async Task<(IEnumerable<CompetitorDto>, int)> GetCompetitors(int competitionId, CompetitorParameters parameters)
    {
        string filterQuery = GetFiltersQuery(
            new[]
            {
                "cp.competition_id = @competitionId",
                @"CONCAT_WS(' ', s.first_name, s.last_name, s.patronymic) " +
                "LIKE @search",
                "cp.belt = @belt",
                "s.sex = @sex",
                "cl.club_id = @clubId",
                "c.coach_id = @coachId",
                "d.division_name = @divisionName",
                "TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) " +
                "BETWEEN @minAge AND @maxAge"
            }, 
            new [] 
            {
                competitionId.ToString(),
                parameters.Search,
                parameters.Belt,
                parameters.Sex,
                parameters.Club,
                parameters.Coach,
                parameters.Division,
                parameters.MaxAge
            });
        
        string query = SelectString
                       + filterQuery
                       + GetSortQuery(parameters.OrderBy)
                       + GetPaginationQuery(parameters.PageNumber, parameters.PageSize);

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var sqlParams = new[]
        {
            new Tuple<string, object>("@competitionId", competitionId),
            new Tuple<string, object>("@search", "%parameters.Search%"),
            new Tuple<string, object>("@belt", parameters.Belt),
            new Tuple<string, object>("@sex", parameters.Sex),
            new Tuple<string, object>("@clubId", parameters.Club),
            new Tuple<string, object>("@coachId", parameters.Coach),
            new Tuple<string, object>("@divisionName", parameters.Division),
            new Tuple<string, object>("@minAge", parameters.MinAge),
            new Tuple<string, object>("@maxAge", parameters.MaxAge),
        };
        
        var competitors = await ReadData<CompetitorDto>(query, connection, sqlParams);
        var total = await Count(SelectString.Split("FROM")[1], connection, sqlParams);

        return (competitors, total);
    }

    public async Task<CompetitorDto?> GetCompetitor(object key)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        string query = SelectString + 
                       $" WHERE application_num = {key}";

        var competitor = await ReadSingle<CompetitorDto>(query, connection);

        return competitor;
    }

    public async Task CreateCompetitors(Competitor[] competitors)
    {
        var paramsToInsert = new StringBuilder("");

        for (int i = 0; i < competitors.Length; i++)
        {
            paramsToInsert.Append($"(@membershipCardNum{i}, @competitionId{i}, @belt{i})");
            paramsToInsert.Append(i != competitors.Length - 1 ? ", " : ";");
        }

        var query =
            $"INSERT INTO competitors(membership_card_num, competition_id, belt) VALUES {paramsToInsert}";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var command = new MySqlCommand(query, connection);

        for (int i = 0; i < competitors.Length; i++)
        {
            command.Parameters.AddRange(new []
            {
                new MySqlParameter($"@membershipCardNum{i}", competitors[i].MembershipCardNum),
                new MySqlParameter($"@competitionId{i}", competitors[i].CompetitionId),
                new MySqlParameter($"@belt{i}", competitors[i].Belt),
            });
        }

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<CompetitorDto>> GetByMembershipCardNums(int competitionId, int[] membershipCardNum)
    {
        string query = SelectString + 
                       " WHERE cp.competition_id = @competitionId" +
                       " AND cp.membership_card_num IN (@membershipCardNum)";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var competitors = await ReadData<CompetitorDto>(query, connection, new []
        {
            new Tuple<string, object>("@competitionId", competitionId), 
            new Tuple<string, object>("@membershipCardNum", String.Join(", ", membershipCardNum)), 
        });

        return competitors;
    }
}