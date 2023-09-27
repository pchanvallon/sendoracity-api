using System.Text.Json.Serialization;

namespace SendoraCityApi.Configuration;

public class RepositoryConfiguration : IRepositoryConfiguration
{
    [JsonPropertyName("POSTGRES_SERVER")]
    private string PostgresServer { get; init; }

    [JsonPropertyName("POSTGRES_DB")]
    private string PostgresDb { get; init; }

    [JsonPropertyName("POSTGRES_USER")]
    private string PostgresUser { get; init; }

    [JsonPropertyName("POSTGRES_PASSWORD")]
    private string PostgresPwd { get; init; }

    public string GetSqlConnectionString() => $"Server={PostgresServer};Database={PostgresDb};User Id={PostgresUser};Password={PostgresPwd};";

    public RepositoryConfiguration(IConfiguration configuration)
    {
        PostgresServer = configuration.GetValue<string>("POSTGRES_SERVER")!;
        PostgresDb = configuration.GetValue<string>("POSTGRES_DB")!;
        PostgresUser = configuration.GetValue<string>("POSTGRES_USER")!;
        PostgresPwd = configuration.GetValue<string>("POSTGRES_PASSWORD")!;
    }

}
