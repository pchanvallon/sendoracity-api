using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public interface ICitiesRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityByIdAsync(int id);
    Task<City?> AddCityAsync(City request);
    Task<City?> UpdateCityAsync(City request);
    Task<City?> DeleteCityAsync(City request);
}
