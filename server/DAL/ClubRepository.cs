using Core.Entities;
using DALAbstractions;
using MySql.Data.MySqlClient;

namespace DAL;

public class ClubRepository: GenericRepository<Club>, IClubRepository
{
    public ClubRepository(string connectionString) 
        : base(connectionString) {}

    public async Task<IEnumerable<Club>> GetClubs()
    {
        string query = "SELECT * FROM clubs";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var clubs = await ReadData(query, connection);

        return clubs;
    }
}