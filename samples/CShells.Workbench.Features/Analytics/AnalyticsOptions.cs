namespace CShells.Workbench.Features.Analytics;
/// <summary>
/// Per-shell configuration for the Analytics feature.
/// Set via inline feature config in appsettings.json.
/// </summary>
public class AnalyticsOptions
{
    /// <summary>
    /// How many top posts to include in the analytics summary.
    /// </summary>
    public int TopPostsCount { get; set; } = 5;
}
