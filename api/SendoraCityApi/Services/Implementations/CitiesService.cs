using SendoraCityApi.Repositories;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public class CitiesService : ICitiesService
{
    protected readonly ICitiesRepository _citiesRepository;
    protected readonly IHousesRepository _housesRepository;
    protected readonly IStoresRepository _storesRepository;

    public CitiesService(
        ICitiesRepository citiesRepository,
        IHousesRepository housesRepository,
        IStoresRepository storesRepository
    )
    {
        _citiesRepository = citiesRepository;
        _housesRepository = housesRepository;
        _storesRepository = storesRepository;
    }

    public async Task<IEnumerable<CityResponse>> GetCitiesAsync()
        => (await _citiesRepository.GetCitiesAsync()).Select(city => new CityResponse(city));

    public async Task<CityResponse?> GetCityByIdAsync(int id)
        => new CityResponse(await GetCityOrThrowException(id));

    public async Task<CityResponse?> AddCityAsync(CityCreateRequest request)
    {
        if ((await _citiesRepository.GetCitiesAsync()).Where(city => city.Name == request.Name!).Any())
        {
            throw new InvalidOperationException($"City with name {request.Name} already exists");
        }

        return new CityResponse((await _citiesRepository.AddCityAsync(new City
        {
            Id = default,
            Name = request.Name!,
            Touristic = request.Touristic! ?? default
        }))!);
    }

    public async Task<CityResponse?> UpdateCityAsync(int id, CityUpdateRequest request)
    {
        var city = await GetCityOrThrowException(id);

        city.Name = request.Name ?? city.Name;
        city.Touristic = request.Touristic ?? city.Touristic;

        return new CityResponse((await _citiesRepository.UpdateCityAsync(city))!);
    }

    public async Task<CityResponse?> DeleteCityAsync(int id)
    {
        var city = await GetCityOrThrowException(id);

        if ((await _housesRepository.GetHousesByCityIdAsync(id)).Any()
            || (await _storesRepository.GetStoresByCityIdAsync(id)).Any())
        {
            throw new InvalidOperationException($"City with id {id} still has buildings");
        }

        return new CityResponse((await _citiesRepository.DeleteCityAsync(city))!);
    }

    private async Task<City> GetCityOrThrowException(int id)
    {
        var city = await _citiesRepository.GetCityByIdAsync(id);
        if (city is null)
        {
            throw new ArgumentOutOfRangeException($"City with id {id} not found");
        }
        return city;
    }
}