using SendoraCityApi.Repositories;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public class StoresService : BuildingsService, IStoresService
{
    public StoresService(
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

    public async Task<IEnumerable<StoreResponse>> GetStoresAsync()
        => (await _storesRepository.GetStoresAsync()).Select(store => new StoreResponse(store));

    public async Task<IEnumerable<StoreResponse>> GetStoresByCityIdAsync(int id)
        => (await _storesRepository.GetStoresByCityIdAsync(id)).Select(store => new StoreResponse(store));

    public async Task<IEnumerable<StoreResponse>> GetStoresByCityNameAsync(string name)
        => (await _storesRepository.GetStoresByCityNameAsync(name)).Select(store => new StoreResponse(store));

    public async Task<IEnumerable<StoreResponse>> GetStoresByCityIdAndTypeAsync(int id, string type)
        => (await _storesRepository.GetStoresByCityIdAndTypeAsync(id, type)).Select(store => new StoreResponse(store));

    public async Task<IEnumerable<StoreResponse>> GetStoresByCityNameAndTypeAsync(string name, string type)
        => (await _storesRepository.GetStoresByCityNameAndTypeAsync(name, type)).Select(store => new StoreResponse(store));

    public async Task<IEnumerable<StoreResponse>> GetStoresByTypeAsync(string type)
        => (await _storesRepository.GetStoresByTypeAsync(type)).Select(store => new StoreResponse(store));

    public async Task<StoreResponse?> GetStoreByIdAsync(int id)
        => new StoreResponse(await GetStoreOrThrowException(id));

    public async Task<StoreResponse?> AddStoreAsync(StoreCreateRequest request)
    {
        var city = await GetCityOrThrowException(request.Cityid ?? default);
        await CheckAddressOrThrowException(city.Id, request.Address!);

        return new StoreResponse((await _storesRepository.AddStoreAsync(new Store
        {
            Name = request.Name!,
            Type = request.Type!,
            Address = request.Address!,
            Cityid = request.Cityid,
        }))!);
    }

    public async Task<StoreResponse?> UpdateStoreAsync(int id, StoreUpdateRequest request)
    {
        var store = await GetStoreOrThrowException(id);

        if (request.Cityid is not null || request.Address is not null)
        {
            var city = await GetCityOrThrowException(request.Cityid ?? store.Cityid ?? default);
            await CheckAddressOrThrowException(city.Id, request.Address ?? store.Address);
        }

        store.Name = request.Name ?? store.Name;
        store.Type = request.Type ?? store.Type;
        store.Address = request.Address ?? store.Address;
        store.Cityid = request.Cityid ?? store.Cityid;

        return new StoreResponse((await _storesRepository.UpdateStoreAsync(store))!);
    }

    public async Task<StoreResponse?> DeleteStoreAsync(int id)
        => new StoreResponse((await _storesRepository.DeleteStoreAsync(await GetStoreOrThrowException(id)))!);

    private async Task<Store> GetStoreOrThrowException(int id)
    {
        var store = await _storesRepository.GetStoreByIdAsync(id);
        if (store is null)
        {
            throw new ArgumentOutOfRangeException($"Store with id {id} not found");
        }
        return store;
    }
}