using CShells.Hosting;

namespace CShells.Workbench.Background;

/// <summary>
/// A background service that demonstrates how to use <see cref="IShellHost"/> and
/// <see cref="IShellContextScopeFactory"/> to execute work within each shell's
/// isolated service provider.
/// </summary>
public class ShellDemoWorker(
    IShellHost shellHost,
    IShellContextScopeFactory scopeFactory,
    ILogger<ShellDemoWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var shell in shellHost.AllShells)
            {
                try
                {
                    ExecuteForShell(shell);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during background heartbeat for shell {ShellId}", shell.Id);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private void ExecuteForShell(ShellContext shell)
    {
        using var scope = scopeFactory.CreateScope(shell);

        // Demonstrate accessing shell-scoped IConfiguration for per-shell settings
        var config = scope.ServiceProvider.GetService<IConfiguration>();
        var plan   = config?["Plan"] ?? "Unknown";

        logger.LogInformation(
            "Heartbeat for shell '{ShellName}' (Plan: {Plan}, Features: {Features})",
            shell.Id.Name,
            plan,
            string.Join(", ", shell.Settings.EnabledFeatures));
    }
}
