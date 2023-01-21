using Core.Entities;
using DALAbstractions;
using MySql.Data.MySqlClient;

namespace DAL;

public class ClubRepository: IClubRepository
{
    private readonly string _connectionString;

    public ClubRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Club>> GetClubs()
    {
        var clubs = new List<Club>(); 
        string query = "SELECT * FROM clubs";

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand(query, connection);
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            clubs.Add(new Club()
            {
                ClubId = await reader.GetFieldValueAsync<int>(0),
                Name = await reader.GetFieldValueAsync<string>(1),
                City = await reader.GetFieldValueAsync<string>(2),
                GymAddr = await reader.GetFieldValueAsync<string>(3),
            });
        }

        return clubs;
    }
}