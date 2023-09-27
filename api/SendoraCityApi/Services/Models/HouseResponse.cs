namespace SendoraCityApi.Services.Models;

public partial class HouseResponse : BuildingResponse
{
    public int Inhabitants { get; init; }

    public HouseResponse(Repositories.Database.Models.House house) : base(house)
    {
        Inhabitants = house.Inhabitants;
    }
}
