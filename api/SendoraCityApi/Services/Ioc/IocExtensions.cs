using SendoraCityApi.Repositories.Ioc;

namespace SendoraCityApi.Services.Ioc;

public static class IocExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddRepositories(configuration)
            .AddTransient<ICitiesService, CitiesService>()
            .AddTransient<IHousesService, HousesService>()
            .AddTransient<IStoresService, StoresService>();
}
