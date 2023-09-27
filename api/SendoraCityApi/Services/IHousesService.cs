using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public interface IHousesService
{
    Task<IEnumerable<HouseResponse>> GetHousesAsync();
    Task<IEnumerable<HouseResponse>> GetHousesByCityIdAsync(int id);
    Task<IEnumerable<HouseResponse>> GetHousesByCityNameAsync(string name);
    Task<HouseResponse?> GetHouseByIdAsync(int id);
    Task<HouseResponse?> AddHouseAsync(HouseCreateRequest request);
    Task<HouseResponse?> UpdateHouseAsync(int id, HouseUpdateRequest request);
    Task<HouseResponse?> DeleteHouseAsync(int id);
}