using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Customers;

public static class ProfileImageHelper
{
    private const string DefaultAvatarPath = "wwwroot/img/default-avatar.svg";
    private static string? _defaultBase64;

    public static string ConvertToBase64(IFormFile profileImage)
    {
        using var memoryStream = new MemoryStream();
        profileImage.CopyTo(memoryStream);

        return BuildDataUri(memoryStream.ToArray(), profileImage.ContentType);
    }

    public static string NormalizeProfileImage(string? storedValue)
    {
        if (!string.IsNullOrWhiteSpace(storedValue))
        {
            if (storedValue.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return storedValue;
            }

            var potentialPath = storedValue.StartsWith("/")
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", storedValue.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()))
                : Path.Combine(Directory.GetCurrentDirectory(), storedValue.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(potentialPath))
            {
                var bytes = File.ReadAllBytes(potentialPath);
                var contentType = GetContentType(potentialPath);

                return BuildDataUri(bytes, contentType);
            }

            return storedValue;
        }

        return GetDefaultBase64();
    }

    public static string GetDefaultBase64()
    {
        if (!string.IsNullOrEmpty(_defaultBase64))
        {
            return _defaultBase64;
        }

        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), DefaultAvatarPath.Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (!File.Exists(avatarPath))
        {
            _defaultBase64 = string.Empty;
            return _defaultBase64;
        }

        var bytes = File.ReadAllBytes(avatarPath);
        var contentType = GetContentType(avatarPath);
        _defaultBase64 = BuildDataUri(bytes, contentType);

        return _defaultBase64;
    }

    private static string BuildDataUri(byte[] bytes, string contentType)
        => $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";

    private static string GetContentType(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}
