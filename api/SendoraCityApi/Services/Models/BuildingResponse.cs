namespace SendoraCityApi.Services.Models;

public partial class BuildingResponse
{
    public int Id { get; init; }
    public string Address { get; init; } = null!;
    public int? Cityid { get; init; }

    public BuildingResponse(Repositories.Database.Models.Building building)
    {
        Id = building.Id;
        Address = building.Address;
        Cityid = building.Cityid;
    }
}
