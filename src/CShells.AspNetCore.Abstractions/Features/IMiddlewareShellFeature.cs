using CShells.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CShells.AspNetCore.Features;

/// <summary>
/// Extends <see cref="IShellFeature"/> with ASP.NET Core middleware registration.
/// </summary>
/// <remarks>
/// <para>
/// Middleware shell features allow shell features to register middleware components
/// into the ASP.NET Core request pipeline. Unlike <see cref="IWebShellFeature"/> which
/// registers endpoints, this interface registers middleware that runs before endpoint dispatch.
/// </para>
/// <para>
/// Middleware registered through this interface is automatically scoped to the shell's
/// path prefix (if configured). The shell's service provider is already set as
/// <c>HttpContext.RequestServices</c> by the time the middleware executes.
/// </para>
/// </remarks>
public interface IMiddlewareShellFeature : IShellFeature
{
    /// <summary>
    /// Registers middleware components for this feature within the shell's pipeline scope.
    /// </summary>
    /// <param name="app">
    /// The application builder. Middleware registered here will be scoped to the shell's
    /// path prefix (if configured) and will execute within the shell's service provider context.
    /// </param>
    /// <param name="environment">The hosting environment, or null if not registered in the service provider.</param>
    /// <remarks>
    /// <para>
    /// This method is called when shells are activated, either during application startup
    /// or when shells are dynamically added at runtime.
    /// </para>
    /// <para>
    /// Middleware registered here runs after the shell resolution middleware has set
    /// <c>HttpContext.RequestServices</c> to the shell's service provider, so any
    /// services resolved from the request will come from the correct shell scope.
    /// </para>
    /// </remarks>
    void UseMiddleware(IApplicationBuilder app, IHostEnvironment? environment);
}

