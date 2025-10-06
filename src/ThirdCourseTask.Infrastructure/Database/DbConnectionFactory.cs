using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ThirdCourseTask.Infrastructure.Database;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _sqlConnectionString;
    private readonly string _pgConnectionString;
    private readonly DbProvider _provider;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _sqlConnectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing 'SqlServer' connection string.");

        _pgConnectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Missing 'Postgres' connection string.");

        string? providerFromEnv = Environment.GetEnvironmentVariable("DB_PROVIDER");

        if (providerFromEnv != null && providerFromEnv.ToLowerInvariant() == "postgres")
        {
            _provider = DbProvider.Postgres;
        }
        else
        {
            _provider = DbProvider.SqlServer;
        }
    }

    public IDbConnection Create()
    {
        if (_provider == DbProvider.Postgres)
        {
            return new NpgsqlConnection(_pgConnectionString);
        }

        if (_provider == DbProvider.SqlServer)
        {
            return new SqlConnection(_sqlConnectionString);
        }

        throw new InvalidOperationException("Unsupported provider: " + _provider);
    }
}
