using CShells.AspNetCore.Features;
using CShells.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace CShells.Workbench.Features.Core;
/// <summary>
/// Core feature — always required.
/// Registers tenant identity and exposes a GET / info endpoint.
/// </summary>
[ShellFeature("Core", DisplayName = "Core", Description = "Core tenant services")]
public class CoreFeature(ShellSettings shellSettings) : IWebShellFeature
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITenantInfo>(new TenantInfo
        {
            TenantId   = shellSettings.Id.ToString(),
            TenantName = shellSettings.Id.ToString(),
            Plan       = shellSettings.ConfigurationData.TryGetValue("Plan", out var plan)
                             ? plan?.ToString() ?? "Free"
                             : "Free"
        });
    }
    public void MapEndpoints(IEndpointRouteBuilder endpoints, IHostEnvironment? environment)
    {
        endpoints.MapGet("", (HttpContext ctx) =>
        {
            var tenant = ctx.RequestServices.GetRequiredService<ITenantInfo>();
            return Results.Ok(new
            {
                tenant   = tenant.TenantName,
                tenantId = tenant.TenantId,
                plan     = tenant.Plan,
                features = shellSettings.EnabledFeatures
            });
        });
    }
}
