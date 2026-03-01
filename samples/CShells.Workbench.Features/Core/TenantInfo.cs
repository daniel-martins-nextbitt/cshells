namespace CShells.Workbench.Features.Core;
/// <summary>
/// Default implementation of <see cref="ITenantInfo"/>.
/// </summary>
public class TenantInfo : ITenantInfo
{
    public string TenantId { get; init; } = "";
    public string TenantName { get; init; } = "";
    public string Plan { get; init; } = "Free";
}
