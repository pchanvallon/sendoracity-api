using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SendoraCityApi.Services.Ioc;

namespace SendoraCityApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
        => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
        => services
            .Configure<MvcOptions>(x =>
            {
                x.Filters.Add(new ProducesAttribute("application/json"));
                x.Filters.Add(new ConsumesAttribute("application/json"));
            })
            .AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .Services
            .AddServices(_configuration)
            .AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "SendoraCityApi",
            }));

    public void Configure(IApplicationBuilder app)
        => app
            .UseRouting()
            .UseEndpoints(x => x.MapControllers())
            .UseSwagger(x => x.RouteTemplate = "swagger/{documentName}/swagger.json")
            .UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint($"/swagger/v1/swagger.json", "SendoraCityApi");
                x.RoutePrefix = "swagger";
            });
}
