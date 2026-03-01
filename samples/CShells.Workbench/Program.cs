using CShells.AspNetCore.Extensions;
using CShells.Workbench.Background;
using CShells.Workbench.Features.Core;

var builder = WebApplication.CreateBuilder(args);

// Load shells from appsettings.json — three tenants with escalating feature tiers.
// Pass the CoreFeature marker type so CShells scans the features assembly.
builder.AddShells([typeof(CoreFeature)]);

// Background service that logs a heartbeat for each active shell every 30 s.
// Demonstrates IShellHost + IShellContextScopeFactory for background work.
builder.Services.AddHostedService<ShellDemoWorker>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapShells();
app.Run();

// Make Program class accessible for WebApplicationFactory in end-to-end tests
namespace CShells.Workbench
{
    public partial class Program;
}