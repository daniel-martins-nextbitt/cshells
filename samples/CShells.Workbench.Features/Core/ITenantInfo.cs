namespace CShells.Workbench.Features.Core;
/// <summary>
/// Provides information about the current tenant (shell).
/// </summary>
public interface ITenantInfo
{
    string TenantId { get; }
    string TenantName { get; }
    string Plan { get; }
}
