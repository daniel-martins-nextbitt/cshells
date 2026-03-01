using System.Collections.Concurrent;
namespace CShells.Workbench.Features.Posts;
/// <summary>
/// Simple, thread-safe in-memory post store seeded with sample data.
/// Each shell gets its own isolated instance.
/// </summary>
public class InMemoryPostRepository : IPostRepository
{
    private readonly ConcurrentDictionary<int, Post> _posts = new();
    private int _nextId = 1;
    public InMemoryPostRepository()
    {
        Add("Hello World", "Welcome to our blog! This is the first post.", "Admin");
        Add("Getting Started", "Here is everything you need to know to get up and running.", "Admin");
    }
    public IReadOnlyList<Post> GetAll() => [.. _posts.Values.OrderBy(p => p.Id)];
    public Post? GetById(int id) => _posts.GetValueOrDefault(id);
    public Post Add(string title, string body, string author)
    {
        var id = Interlocked.Increment(ref _nextId) - 1;
        var post = new Post(id, title, body, author, DateTimeOffset.UtcNow);
        _posts[id] = post;
        return post;
    }
}
