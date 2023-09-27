using SendoraCityApi.Repositories;
using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Services;

public class BuildingsService
{
    protected readonly ICitiesRepository _citiesRepository;
    protected readonly IHousesRepository _housesRepository;
    protected readonly IStoresRepository _storesRepository;

    public BuildingsService(
        ICitiesRepository citiesRepository,
        IHousesRepository housesRepository,
        IStoresRepository storesRepository
    )
    {
        _citiesRepository = citiesRepository;
        _housesRepository = housesRepository;
        _storesRepository = storesRepository;
    }


    protected async Task<City> GetCityOrThrowException(int id)
    {
        var city = await _citiesRepository.GetCityByIdAsync(id);
        if (city is null)
        {
            throw new ArgumentException($"City with id {id} not found");
        }
        return city;
    }

    protected async Task CheckAddressOrThrowException(int cityId, string address)
    {
        if ((await _housesRepository.GetHousesByCityIdAsync(cityId)).Any(x => x.Address.ToLower() == address.ToLower())
            || (await _storesRepository.GetStoresByCityIdAsync(cityId)).Any(x => x.Address.ToLower() == address.ToLower()))
        {
            throw new ArgumentException($"Building with address {address} already exists in this city ({cityId})");
        }
    }
}