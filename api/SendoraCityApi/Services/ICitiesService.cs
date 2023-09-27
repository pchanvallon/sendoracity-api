using SendoraCityApi.Services.Models;

namespace SendoraCityApi.Services;

public interface ICitiesService
{
    Task<IEnumerable<CityResponse>> GetCitiesAsync();
    Task<CityResponse?> GetCityByIdAsync(int id);
    Task<CityResponse?> AddCityAsync(CityCreateRequest request);
    Task<CityResponse?> UpdateCityAsync(int id, CityUpdateRequest request);
    Task<CityResponse?> DeleteCityAsync(int id);
}