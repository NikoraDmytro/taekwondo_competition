using MySql.Data.MySqlClient;

namespace DALAbstractions;

public interface IGenericRepository<TEntity>
{
    MySqlConnection CreateConnection();
    
    Task<IEnumerable<TEntity>> ReadData(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters);
    
    Task Add(TEntity entity);
}