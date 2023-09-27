using Microsoft.EntityFrameworkCore;
using SendoraCityApi.Configuration;
using SendoraCityApi.Repositories.Database.Models;

namespace SendoraCityApi.Repositories.Ioc;

public static class IocExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IRepositoryConfiguration>(new RepositoryConfiguration(configuration))
            .AddDbContext<SendoraDbContext>(options =>
            {
                var configuration = services.BuildServiceProvider().GetService<IRepositoryConfiguration>();
                options.UseNpgsql(configuration?.GetSqlConnectionString());
            })
            .AddTransient<ICitiesRepository, CitiesRepository>()
            .AddTransient<IHousesRepository, HousesRepository>()
            .AddTransient<IStoresRepository, StoresRepository>();
}
