using Microsoft.EntityFrameworkCore;
using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public class HousesRepository : IHousesRepository
{
    private readonly SendoraDbContext _context;

    public HousesRepository(SendoraDbContext context)
        => _context = context;

    public async Task<IEnumerable<House>> GetHousesAsync()
        => await _context.Houses.ToListAsync();

    public async Task<IEnumerable<House>> GetHousesByCityIdAsync(int id)
        => await _context.Houses.Where(x => x.Cityid == id).ToListAsync();

    public async Task<IEnumerable<House>> GetHousesByCityNameAsync(string name)
        => await _context.Houses.Where(x => x.City!.Name.ToLower() == name.ToLower()).ToListAsync();

    public async Task<House?> GetHouseByIdAsync(int id)
        => await _context.Houses.Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<House?> AddHouseAsync(House request)
    {
        var house = await _context.Houses.AddAsync(request);
        await _context.SaveChangesAsync();
        return house.Entity;
    }

    public async Task<House?> UpdateHouseAsync(House request)
    {
        var house = _context.Houses.Update(request);
        await _context.SaveChangesAsync();
        return house.Entity;
    }

    public async Task<House?> DeleteHouseAsync(House request)
    {
        var house = _context.Houses.Remove(request);
        await _context.SaveChangesAsync();
        return house.Entity;
    }
}