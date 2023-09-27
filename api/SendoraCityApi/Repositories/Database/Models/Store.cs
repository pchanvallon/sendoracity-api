namespace SendoraCityApi.Repositories.Database.Models;

public partial class Store : Building
{
    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;
}
