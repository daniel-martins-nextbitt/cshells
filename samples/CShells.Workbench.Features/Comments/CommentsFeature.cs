using CShells.AspNetCore.Features;
using CShells.Features;
using CShells.Workbench.Features.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace CShells.Workbench.Features.Comments;
/// <summary>
/// Comments feature — adds reader comments to blog posts.
/// Requires the Posts feature.
/// Exposes GET /posts/{id}/comments and POST /posts/{id}/comments.
/// </summary>
[ShellFeature("Comments", DependsOn = ["Posts"], DisplayName = "Post Comments")]
public class CommentsFeature : IWebShellFeature
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICommentRepository, InMemoryCommentRepository>();
    }
    public void MapEndpoints(IEndpointRouteBuilder endpoints, IHostEnvironment? environment)
    {
        endpoints.MapGet("/posts/{id:int}/comments", (int id, HttpContext ctx) =>
        {
            var repo     = ctx.RequestServices.GetRequiredService<ICommentRepository>();
            var comments = repo.GetByPostId(id);
            return Results.Ok(comments);
        });
        endpoints.MapPost("/posts/{id:int}/comments", async (int id, HttpContext ctx) =>
        {
            var req = await ctx.Request.ReadFromJsonAsync<CreateCommentRequest>();
            if (req is null) return Results.BadRequest();
            var repo    = ctx.RequestServices.GetRequiredService<ICommentRepository>();
            var comment = repo.Add(id, req.Author, req.Body);
            return Results.Created($"/posts/{id}/comments", comment);
        });
    }
}
public record CreateCommentRequest(string Author, string Body);
