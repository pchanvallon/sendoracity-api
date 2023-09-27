using SendoraCityApi.Repositories;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public class HousesService : BuildingsService, IHousesService
{
    public HousesService(
        ICitiesRepository citiesRepository,
        IHousesRepository housesRepository,
        IStoresRepository storesRepository
    )
    : base(
        citiesRepository,
        housesRepository,
        storesRepository
    )
    { }

    public async Task<IEnumerable<HouseResponse>> GetHousesAsync()
        => (await _housesRepository.GetHousesAsync()).Select(house => new HouseResponse(house));

    public async Task<IEnumerable<HouseResponse>> GetHousesByCityIdAsync(int id)
        => (await _housesRepository.GetHousesByCityIdAsync(id)).Select(house => new HouseResponse(house));

    public async Task<IEnumerable<HouseResponse>> GetHousesByCityNameAsync(string name)
        => (await _housesRepository.GetHousesByCityNameAsync(name)).Select(house => new HouseResponse(house));

    public async Task<HouseResponse?> GetHouseByIdAsync(int id)
        => new HouseResponse(await GetHouseOrThrowException(id));

    public async Task<HouseResponse?> AddHouseAsync(HouseCreateRequest request)
    {
        var city = await GetCityOrThrowException(request.Cityid ?? default);
        await CheckAddressOrThrowException(city.Id, request.Address!);

        return new HouseResponse((await _housesRepository.AddHouseAsync(new House
        {
            Address = request.Address!,
            Cityid = request.Cityid,
            Inhabitants = request.Inhabitants ?? default
        }))!);
    }

    public async Task<HouseResponse?> UpdateHouseAsync(int id, HouseUpdateRequest request)
    {
        var house = await GetHouseOrThrowException(id);

        if (request.Cityid is not null || request.Address is not null)
        {
            var city = await GetCityOrThrowException(request.Cityid ?? house.Cityid ?? default);
            await CheckAddressOrThrowException(city.Id, request.Address ?? house.Address);
        }

        house.Address = request.Address ?? house.Address;
        house.Cityid = request.Cityid ?? house.Cityid;
        house.Inhabitants = request.Inhabitants ?? house.Inhabitants;

        return new HouseResponse((await _housesRepository.UpdateHouseAsync(house))!);
    }

    public async Task<HouseResponse?> DeleteHouseAsync(int id)
        => new HouseResponse((await _housesRepository.DeleteHouseAsync(await GetHouseOrThrowException(id)))!);

    private async Task<House> GetHouseOrThrowException(int id)
    {
        var house = await _housesRepository.GetHouseByIdAsync(id);
        if (house is null)
        {
            throw new ArgumentOutOfRangeException($"House with id {id} not found");
        }
        return house;
    }
}