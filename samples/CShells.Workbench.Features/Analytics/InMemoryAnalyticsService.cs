using System.Collections.Concurrent;
namespace CShells.Workbench.Features.Analytics;
/// <summary>
/// Thread-safe in-memory view counter. Isolated per shell.
/// </summary>
public class InMemoryAnalyticsService : IAnalyticsService
{
    private readonly ConcurrentDictionary<int, long> _views = new();
    public void RecordView(int postId) =>
        _views.AddOrUpdate(postId, 1, (_, count) => count + 1);
    public IReadOnlyDictionary<int, long> GetViewCounts() => _views;
}
