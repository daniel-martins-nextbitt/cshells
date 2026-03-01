using CShells.AspNetCore.Features;
using CShells.Features;
using CShells.Workbench.Features.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
namespace CShells.Workbench.Features.Analytics;
/// <summary>
/// Analytics feature — tracks post view counts and exposes a summary endpoint.
/// Demonstrates IConfigurableFeature&lt;T&gt; for per-shell configuration.
/// </summary>
[ShellFeature("Analytics", DependsOn = ["Posts"], DisplayName = "Post Analytics")]
public class AnalyticsFeature : IWebShellFeature, IConfigurableFeature<AnalyticsOptions>
{
    private AnalyticsOptions _options = new();
    /// <summary>Called automatically after options are bound from configuration.</summary>
    public void Configure(AnalyticsOptions options) => _options = options;
    public void ConfigureServices(IServiceCollection services)
    {
        // Bind feature-specific options from the shell's IConfiguration
        services.AddOptions<AnalyticsOptions>()
            .Configure<IConfiguration>((opts, config) =>
                config.GetSection("Analytics").Bind(opts));
        services.AddSingleton<IAnalyticsService, InMemoryAnalyticsService>();
    }
    public void MapEndpoints(IEndpointRouteBuilder endpoints, IHostEnvironment? environment)
    {
        endpoints.MapGet("/analytics", (HttpContext ctx) =>
        {
            var analytics = ctx.RequestServices.GetRequiredService<IAnalyticsService>();
            var options   = ctx.RequestServices.GetRequiredService<IOptions<AnalyticsOptions>>().Value;
            var tenant    = ctx.RequestServices.GetRequiredService<ITenantInfo>();
            var topPosts = analytics.GetViewCounts()
                .OrderByDescending(kv => kv.Value)
                .Take(options.TopPostsCount)
                .Select(kv => new { postId = kv.Key, views = kv.Value });
            return Results.Ok(new
            {
                tenant          = tenant.TenantName,
                topPostsCount   = options.TopPostsCount,
                topPosts
            });
        });
    }
}
