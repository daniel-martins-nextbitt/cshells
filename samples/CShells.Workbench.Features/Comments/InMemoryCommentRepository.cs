using System.Collections.Concurrent;
namespace CShells.Workbench.Features.Comments;
/// <summary>
/// Thread-safe in-memory comment store. Isolated per shell.
/// </summary>
public class InMemoryCommentRepository : ICommentRepository
{
    private readonly ConcurrentBag<Comment> _comments = [];
    private int _nextId = 1;
    public IReadOnlyList<Comment> GetByPostId(int postId) =>
        [.. _comments.Where(c => c.PostId == postId).OrderBy(c => c.Id)];
    public Comment Add(int postId, string author, string body)
    {
        var comment = new Comment(
            Interlocked.Increment(ref _nextId) - 1,
            postId, author, body, DateTimeOffset.UtcNow);
        _comments.Add(comment);
        return comment;
    }
}
