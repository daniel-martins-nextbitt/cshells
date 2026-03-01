namespace CShells.Workbench.Features.Comments;
public interface ICommentRepository
{
    IReadOnlyList<Comment> GetByPostId(int postId);
    Comment Add(int postId, string author, string body);
}
