using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public interface IStoresRepository
{
    Task<IEnumerable<Store>> GetStoresAsync();
    Task<IEnumerable<Store>> GetStoresByCityIdAsync(int id);
    Task<IEnumerable<Store>> GetStoresByCityNameAsync(string name);
    Task<IEnumerable<Store>> GetStoresByCityIdAndTypeAsync(int id, string type);
    Task<IEnumerable<Store>> GetStoresByCityNameAndTypeAsync(string name, string type);
    Task<IEnumerable<Store>> GetStoresByTypeAsync(string type);
    Task<Store?> GetStoreByIdAsync(int id);
    Task<Store?> AddStoreAsync(Store request);
    Task<Store?> UpdateStoreAsync(Store request);
    Task<Store?> DeleteStoreAsync(Store request);
}
