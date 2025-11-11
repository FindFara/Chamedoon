namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminUserDto(
    long Id,
    string Email,
    string UserName,
    string? FullName,
    long? RoleId,
    string? RoleName,
    bool IsActive,
    DateTime CreatedAt);

public class AdminUserInput
{
    public long? Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public long? RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Password { get; set; }
}

public record MonthlyRegistrationCount(int Year, int Month, int Count);
