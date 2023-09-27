using Microsoft.EntityFrameworkCore;
using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public class CitiesRepository : ICitiesRepository
{
    private readonly SendoraDbContext _context;

    public CitiesRepository(SendoraDbContext context)
        => _context = context;

    public async Task<IEnumerable<City>> GetCitiesAsync()
        => await _context.Cities.ToListAsync();

    public async Task<City?> GetCityByIdAsync(int id)
        => await _context.Cities.Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<City?> AddCityAsync(City request)
    {
        var city = await _context.Cities.AddAsync(request);
        await _context.SaveChangesAsync();
        return city.Entity;
    }

    public async Task<City?> UpdateCityAsync(City request)
    {
        var city = _context.Cities.Update(request);
        await _context.SaveChangesAsync();
        return city.Entity;
    }

    public async Task<City?> DeleteCityAsync(City request)
    {
        var city = _context.Cities.Remove(request);
        await _context.SaveChangesAsync();
        return city.Entity;
    }
}