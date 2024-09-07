using System.Data.Common;
using Microsoft.Data.SqlClient;
using Expense.Tracker.DatabaseService.ConnectionFactory;
using Microsoft.Extensions.Configuration;



Console.WriteLine("***** Fun with Data Provider Factories *****\n");
var (provider, connectionString) = GetProviderFromConfiguration();
DbProviderFactory factory = GetDbProviderFactory(provider);
// Now get the connection object.
using (DbConnection connection = factory.CreateConnection())
{
    Console.WriteLine($"Your connection object is a: {connection.GetType().Name}");
    connection.ConnectionString = connectionString;
    connection.Open();
    // Make command object.
    DbCommand command = factory.CreateCommand();
    Console.WriteLine($"Your command object is a: {command.GetType().Name}");
    command.Connection = connection;
    command.CommandText =
    "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId ";
    // Print out data with data reader.
    using (DbDataReader dataReader = command.ExecuteReader())
    {
        Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
        Console.WriteLine("\n***** Current Inventory *****");
        while (dataReader.Read())
        {
            Console.WriteLine($"-> Car #{dataReader["Id"]} is a {dataReader["Name"]}.");
        }
    }
}
Console.ReadLine();

static DbProviderFactory GetDbProviderFactory(DataProviderEnum provider)
=> provider switch
{
    DataProviderEnum.SqlServer => SqlClientFactory.Instance,
    _ => SqlClientFactory.Instance
};


static (DataProviderEnum Provider, string ConnectionString)
GetProviderFromConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .Build();
    var providerName = config["ProviderName"];
    if (Enum.TryParse<DataProviderEnum>
    (providerName, out DataProviderEnum provider))
    {
        return (provider, config[$"{providerName}:ConnectionString"]);
    };
    throw new Exception("Invalid data provider value supplied.");
}