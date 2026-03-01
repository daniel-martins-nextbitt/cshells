namespace CShells.Workbench.Features.Posts;
public record Post(int Id, string Title, string Body, string Author, DateTimeOffset PublishedAt);
