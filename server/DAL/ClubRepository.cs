using Core.Entities;
using Core.RequestFeatures;
using DALAbstractions;

namespace DAL;

public class ClubRepository: GenericRepository<Club>, IClubRepository
{
    public ClubRepository(string connectionString) 
        : base(connectionString) {}

    public async Task<IEnumerable<Club>> GetClubs(ClubParameters parameters)
    {
        string filterQuery = GetFiltersQuery(
            new[]
            {
                "club_name LIKE @search",
                "city = @city",
            }, 
            new[] 
            {
                parameters.Search,
                parameters.City,
            });

        string query =
            "SELECT * FROM clubs"
            + filterQuery
            + GetSortQuery(parameters.OrderBy);

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var clubs = await ReadData(query, connection, new []
        {
            new Tuple<string, object>("@search", $"%{parameters.Search}%"),
            new Tuple<string, object>("@city", parameters.City),
        });

        return clubs;
    }

    public async Task<Club?> GetClubByName(string name)
    {
        string query = $"SELECT * FROM clubs WHERE club_name = '{name}'";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var club = await ReadSingle(query, connection);

        return club;
    }
}