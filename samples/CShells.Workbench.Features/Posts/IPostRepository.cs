namespace CShells.Workbench.Features.Posts;
public interface IPostRepository
{
    IReadOnlyList<Post> GetAll();
    Post? GetById(int id);
    Post Add(string title, string body, string author);
}
