using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public interface IStoresService
{
    Task<IEnumerable<StoreResponse>> GetStoresAsync();
    Task<IEnumerable<StoreResponse>> GetStoresByCityIdAsync(int id);
    Task<IEnumerable<StoreResponse>> GetStoresByCityNameAsync(string name);
    Task<IEnumerable<StoreResponse>> GetStoresByCityIdAndTypeAsync(int id, string type);
    Task<IEnumerable<StoreResponse>> GetStoresByCityNameAndTypeAsync(string name, string type);
    Task<IEnumerable<StoreResponse>> GetStoresByTypeAsync(string type);
    Task<StoreResponse?> GetStoreByIdAsync(int id);
    Task<StoreResponse?> AddStoreAsync(StoreCreateRequest request);
    Task<StoreResponse?> UpdateStoreAsync(int id, StoreUpdateRequest request);
    Task<StoreResponse?> DeleteStoreAsync(int id);
}