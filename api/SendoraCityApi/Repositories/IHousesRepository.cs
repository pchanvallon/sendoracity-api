using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public interface IHousesRepository
{
    Task<IEnumerable<House>> GetHousesAsync();
    Task<IEnumerable<House>> GetHousesByCityIdAsync(int id);
    Task<IEnumerable<House>> GetHousesByCityNameAsync(string name);
    Task<House?> GetHouseByIdAsync(int id);
    Task<House?> AddHouseAsync(House request);
    Task<House?> UpdateHouseAsync(House request);
    Task<House?> DeleteHouseAsync(House request);
}
