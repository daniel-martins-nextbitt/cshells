namespace CShells.Workbench.Features.Analytics;
public interface IAnalyticsService
{
    void RecordView(int postId);
    IReadOnlyDictionary<int, long> GetViewCounts();
}
