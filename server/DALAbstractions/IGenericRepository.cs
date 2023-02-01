using MySql.Data.MySqlClient;

namespace DALAbstractions;

public interface IGenericRepository<TEntity>
{
    MySqlConnection CreateConnection();
    string GetFiltersQuery(string[] queryParts, string?[] filters);
    string GetSortQuery(string sortParam);
    string GetPaginationQuery(int pageNumber, int pageSize);
    Task<IEnumerable<TEntity>> ReadData(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters);
    Task<TEntity?> ReadSingle(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters);

    Task<int> Count(
        string fromQuery, 
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters);
    Task<TEntity?> GetById(object key);
    Task<IEnumerable<TEntity>> GetByIds(object[] keys); 
    Task<object> Add(TEntity entity);
    Task Delete(object key);
    Task Update(object key, TEntity entity);
}