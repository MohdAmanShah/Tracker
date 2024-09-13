using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Expense.Tracker.DatabaseService.DatabaseProviderFactory;

public static class DbProviderFactoryExtensions
{
    public static DbProviderFactory GetDbProviderFactory(DataProviderEnum provider) =>
     provider switch
     {
         DataProviderEnum.SqlServer => SqlClientFactory.Instance,
         _ => SqlClientFactory.Instance
     };

    public static (DataProviderEnum provider, string connectionString) GetProviderFromConfiguration()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        string providerName = configuration["ProviderName"] ?? "SqlServer";
        if (string.IsNullOrEmpty(providerName))
            throw new ArgumentException("ProviderName is missing in the configuration.");

        if (Enum.TryParse<DataProviderEnum>(providerName, true, out DataProviderEnum provider))
        {
            string connectionString = configuration[$"{providerName}:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"ConnectionString is missing for provider {providerName}.");

            return (provider, connectionString);
        }

        throw new ArgumentException("Invalid data provider value supplied.");
    }
}