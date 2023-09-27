using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Services.Models;

public class CityResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public bool Touristic { get; init; }

    public CityResponse(City city)
    {
        Id = city.Id;
        Name = city.Name;
        Touristic = city.Touristic;
    }
}
