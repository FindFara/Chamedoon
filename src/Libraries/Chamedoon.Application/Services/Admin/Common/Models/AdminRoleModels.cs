namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminRoleDto(
    long Id,
    string Name,
    IReadOnlyList<string> PermissionNames);

public class AdminRoleInput
{
    public long? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<string> PermissionNames { get; set; } = new List<string>();
}
