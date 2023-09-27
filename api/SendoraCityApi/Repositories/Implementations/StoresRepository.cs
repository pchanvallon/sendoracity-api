using Microsoft.EntityFrameworkCore;
using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories;

public class StoresRepository : IStoresRepository
{
    private readonly SendoraDbContext _context;

    public StoresRepository(SendoraDbContext context)
        => _context = context;

    public async Task<IEnumerable<Store>> GetStoresAsync()
        => await _context.Stores.ToListAsync();

    public async Task<IEnumerable<Store>> GetStoresByCityIdAsync(int id)
        => await _context.Stores.Where(x => x.Cityid == id).ToListAsync();

    public async Task<IEnumerable<Store>> GetStoresByCityNameAsync(string name)
        => await _context.Stores.Where(x => x.City!.Name.ToLower() == name.ToLower()).ToListAsync();

    public async Task<IEnumerable<Store>> GetStoresByCityIdAndTypeAsync(int id, string type)
        => await _context.Stores.Where(x => x.Cityid == id && x.Type.ToLower() == type.ToLower()).ToListAsync();

    public async Task<IEnumerable<Store>> GetStoresByCityNameAndTypeAsync(string name, string type)
        => await _context.Stores.Where(x => x.City!.Name.ToLower() == name.ToLower() && x.Type.ToLower() == type.ToLower()).ToListAsync();

    public async Task<IEnumerable<Store>> GetStoresByTypeAsync(string type)
        => await _context.Stores.Where(x => x.Type.ToLower() == type.ToLower()).ToListAsync();

    public async Task<Store?> GetStoreByIdAsync(int id)
        => await _context.Stores.Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Store?> AddStoreAsync(Store request)
    {
        var store = await _context.Stores.AddAsync(request);
        await _context.SaveChangesAsync();
        return store.Entity;
    }

    public async Task<Store?> UpdateStoreAsync(Store request)
    {
        var store = _context.Stores.Update(request);
        await _context.SaveChangesAsync();
        return store.Entity;
    }

    public async Task<Store?> DeleteStoreAsync(Store request)
    {
        var store = _context.Stores.Remove(request);
        await _context.SaveChangesAsync();
        return store.Entity;
    }
}