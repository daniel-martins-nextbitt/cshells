namespace CShells.Workbench.Features.Comments;
public record Comment(int Id, int PostId, string Author, string Body, DateTimeOffset CreatedAt);
