using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using Expense.Tracker.DatabaseService.DatabaseProviderFactory;

namespace Expense.Tracker.DatabaseService.DataOperations;
public class DatabaseOperationsBase : IDisposable
{
    protected string _connectionString;
    protected DataProviderEnum _provider;
    protected DbProviderFactory _factory;
    protected DbConnection _connection;

    public DatabaseOperationsBase() : this(DbProviderFactoryExtensions.GetProviderFromConfiguration())
    {
    }

    public DatabaseOperationsBase((DataProviderEnum provider, string connectionString) provider)
    {
        _connectionString = provider.connectionString;
        _provider = provider.provider;
        _factory = DbProviderFactoryExtensions.GetDbProviderFactory(_provider);
        _connection = _factory.CreateConnection() ?? new SqlConnection();
        _connection.ConnectionString = _connectionString;
    }

    protected void OpenConnection()
    {
        if (_connection?.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }

    protected void CloseConnection()
    {
        if (_connection?.State != ConnectionState.Closed)
        {
            _connection?.Close();
        }
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            _connection?.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected DbParameter CreateParameter(string name, DbType type, object value, ParameterDirection direction = ParameterDirection.Input)
    {
        DbParameter param = _factory.CreateParameter();
        param.ParameterName = name;
        param.DbType = type;
        param.Value = value;
        param.Direction = direction;
        return param;
    }
}
