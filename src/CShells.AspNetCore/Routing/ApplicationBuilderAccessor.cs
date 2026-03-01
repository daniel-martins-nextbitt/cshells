using Microsoft.AspNetCore.Builder;

namespace CShells.AspNetCore.Routing;

/// <summary>
/// Provides access to the IApplicationBuilder captured during MapShells().
/// This allows notification handlers to register shell middleware after application startup.
/// </summary>
public class ApplicationBuilderAccessor
{
    /// <summary>
    /// Gets or sets the application builder.
    /// </summary>
    public IApplicationBuilder? ApplicationBuilder { get; set; }
}

