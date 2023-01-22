using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using DALAbstractions;
using MySql.Data.MySqlClient;

namespace DAL;

public class GenericRepository<TEntity>
    : IGenericRepository<TEntity> where TEntity: class
{
    private readonly string _connectionString;
    
    protected GenericRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection CreateConnection() => new(_connectionString);

    private string ToSnakeCase(string camelCase)
    {
        var newString = new StringBuilder(camelCase[0].ToString());
        
        for (int i = 1; i < camelCase.Length; i++)
        {
            char ch = camelCase[i];
            if (Char.IsUpper(ch))
            {
                newString.Append("_");
            }
            newString.Append(ch);
        }

        return newString.ToString().ToLower();
    }
    
    private async Task<TEntity> ReadRow(DbDataReader reader)
    {
        Type entityType = typeof(TEntity);
        TEntity? entity = Activator.CreateInstance<TEntity>();

        foreach (var property in entityType.GetProperties())
        {
            string name = property.Name;
            string dbName = ToSnakeCase(name);
            PropertyInfo? propertyInfo = entityType.GetProperty(name);
            object value = await reader.GetFieldValueAsync<object>(dbName);
                    
            propertyInfo?.SetValue(entity, value);
        }

        return entity;
    }
    
    public async Task<IEnumerable<TEntity>> ReadData(
        string query, 
        MySqlConnection connection, 
        IEnumerable<Tuple<string, object>>? sqlParameters = null)
    {
        var data = new List<TEntity>();
        var command = new MySqlCommand(query, connection);

        if (sqlParameters != null)
        {
            foreach (var (item1, item2) in sqlParameters)
            {
                var newParam = new MySqlParameter(item1, item2);
                command.Parameters.Add(newParam);
            }
        }
        
        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            TEntity entity = await ReadRow(reader);
            data.Add(entity);
        }

        return data;
    }

    public async Task Add(TEntity entity)
    {
        
    }
}