namespace SendoraCityApi.Services.Models;

public partial class StoreResponse : BuildingResponse
{
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;

    public StoreResponse(Repositories.Database.Models.Store store) : base(store)
    {
        Name = store.Name;
        Type = store.Type;
    }
}
