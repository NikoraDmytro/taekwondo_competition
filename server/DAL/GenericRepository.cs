using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using DALAbstractions;
using MySql.Data.MySqlClient;

namespace DAL;

public class GenericRepository<TEntity>
    : IGenericRepository<TEntity> where TEntity : class
{
    private readonly Type _entityType;
    private readonly string _connectionString;
    private readonly string _primaryKeyName;

    protected GenericRepository(string connectionString)
    {
        _entityType = typeof(TEntity);
        _connectionString = connectionString;

        PropertyInfo keyField = _entityType
            .GetProperties()
            .Single(prop => prop
                .GetCustomAttributes<KeyAttribute>()
                .Any());
        _primaryKeyName = ToSnakeCase(keyField.Name);
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

    private string GetTableName()
    {
        string tableName = ToSnakeCase(_entityType.Name);
        int wordCount = tableName.Split('_').Length;

        if (wordCount > 1)
        {
            return tableName;
        }

        return tableName + 's';
    }

    private async Task<T> ReadRow<T>(DbDataReader reader)
    {
        Type entityType = typeof(T);
        T entity = Activator.CreateInstance<T>();

        foreach (var property in entityType.GetProperties())
        {
            string name = property.Name;
            string dbName = ToSnakeCase(name);
            PropertyInfo? propertyInfo = entityType.GetProperty(name);
            object? value = await reader.GetFieldValueAsync<object?>(dbName);

            if (value == DBNull.Value)
                value = null;

            propertyInfo?.SetValue(entity, value);
        }

        return entity;
    }

    public string GetFiltersQuery(string[] queryParts, string?[] filters)
    {
        var filtersQuery = new List<string>();

        for (int i = 0; i < filters.Length; i++)
        {
            if (!string.IsNullOrEmpty(filters[i]))
            {
                filtersQuery.Add(queryParts[i]);
            }
        }

        if (filtersQuery.Count == 0)
        {
            return "";
        }

        return $" WHERE {String.Join(" AND ", filtersQuery)}";
    }

    public string GetSortQuery(string sortParam)
    {
        if (string.IsNullOrEmpty(sortParam))
        {
            return $" ORDER BY {_primaryKeyName} DESC";
        }

        string[] sortParamParts = sortParam.Split(" ");
        string sortField = sortParamParts[0];

        if (_entityType.GetProperty(
                sortField,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance) == null)
        {
            return $" ORDER BY {_primaryKeyName} DESC";
        }
        sortField = ToSnakeCase(sortField);

        if (sortParamParts.Length == 1)
        {
            return $" ORDER BY {sortField}, {_primaryKeyName} DESC";
        }
        string sortDirection = sortParamParts[1].ToUpper();

        if (sortDirection == "DESC")
        {
            return $" ORDER BY {sortField} DESC, {_primaryKeyName} DESC";
        }

        return $" ORDER BY {sortField} ASC, {_primaryKeyName} DESC";
    }

    public string GetPaginationQuery(int pageNumber, int pageSize)
    {
        return $" LIMIT {(pageNumber - 1) * pageSize},{pageSize}";
    }

    protected async Task<IEnumerable<T>> ReadData<T>(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters = null)
        where T : class
    {
        var data = new List<T>();
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
            T entity = await ReadRow<T>(reader);
            data.Add(entity);
        }

        await reader.CloseAsync();
        
        return data;
    }

    public async Task<IEnumerable<TEntity>> ReadData(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters = null)
    {
        return await ReadData<TEntity>(query, connection, sqlParameters);
    }

    protected async Task<T?> ReadSingle<T>(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters = null)
        where T : class
    {
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
            T entity = await ReadRow<T>(reader);

            return entity;
        }

        await reader.CloseAsync();
        
        return null;
    }

    public async Task<TEntity?> ReadSingle(
        string query,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters = null)
    {
        return await ReadSingle<TEntity>(query, connection, sqlParameters);
    }

    public async Task<int> Count(
        string fromQuery,
        MySqlConnection connection,
        IEnumerable<Tuple<string, object>>? sqlParameters)
    {
        var query = $"SELECT COUNT(*) FROM {fromQuery}";
        var command = new MySqlCommand(query, connection);

        if (sqlParameters != null)
        {
            foreach (var (item1, item2) in sqlParameters)
            {
                var newParam = new MySqlParameter(item1, item2);
                command.Parameters.Add(newParam);
            }
        }

        object? count = await command.ExecuteScalarAsync();

        return (int)(Int64)(count ?? 0);
    }

    public async Task<TEntity?> GetById(object key)
    {
        string tableName = GetTableName();

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        string query = $"SELECT * FROM {tableName} WHERE {_primaryKeyName} = {key}";

        var foundEntity = await ReadSingle(query, connection);

        return foundEntity;
    }
    
    public async Task<IEnumerable<TEntity>> GetByIds(object[] keys)
    {
        string tableName = GetTableName();

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        string query = $"SELECT * FROM {tableName} WHERE {_primaryKeyName} IN ({String.Join(", ", keys)})";

        var foundEntities = await ReadData(query, connection);

        return foundEntities;
    }
    
    public async Task<object> Add(TEntity entity)
    {
        var names = new List<string>();
        var values = new List<object>();
        var paramNames = new List<string>();

        foreach (var property in _entityType.GetProperties())
        {
            string name = property.Name;
            string dbName = ToSnakeCase(name);
            PropertyInfo? propertyInfo = _entityType.GetProperty(name);
            object? value = propertyInfo?.GetValue(entity);

            if (value != null && dbName != _primaryKeyName)
            {
                names.Add(dbName);
                values.Add(value);
                paramNames.Add("@" + name);
            }
        }

        string tableName = GetTableName();
        string namesString = String.Join(",", names);
        string paramsString = String.Join(",", paramNames);
        string query =
            $"INSERT INTO {tableName} ({namesString}) VALUES({paramsString});" +
            "SELECT LAST_INSERT_ID();";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new MySqlCommand(query, connection);

        for (int i = 0; i < paramNames.Count(); i++)
        {
            var valuesParam = new MySqlParameter(paramNames[i], values[i]);
            command.Parameters.Add(valuesParam);
        }

        object newKey = (await command.ExecuteScalarAsync())!;

        return newKey;
    }

    public async Task Delete(object key)
    {
        string query = $"DELETE FROM {GetTableName()} WHERE {_primaryKeyName} = {key}";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new MySqlCommand(query, connection);

        await command.ExecuteNonQueryAsync();
    }

    public async Task Update(object key, TEntity entity)
    {
        var values = new List<object>();
        var setFields = new List<string>();
        var paramNames = new List<string>();

        foreach (var property in _entityType.GetProperties())
        {
            string name = property.Name;
            string dbName = ToSnakeCase(name);
            PropertyInfo? propertyInfo = _entityType.GetProperty(name);
            object? value = propertyInfo?.GetValue(entity);

            if (value != null && dbName != _primaryKeyName)
            {
                values.Add(value);
                paramNames.Add("@" + name);
                setFields.Add($"{dbName} = @{name}");
            }
        }

        string tableName = GetTableName();
        string setFieldsString = String.Join(",", setFields);
        string query = $"UPDATE {tableName} SET {setFieldsString} " +
                       $"WHERE {_primaryKeyName} = {key}";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var command = new MySqlCommand(query, connection);

        for (int i = 0; i < paramNames.Count(); i++)
        {
            var valuesParam = new MySqlParameter(paramNames[i], values[i]);
            command.Parameters.Add(valuesParam);
        }

        await command.ExecuteNonQueryAsync();
    }
}