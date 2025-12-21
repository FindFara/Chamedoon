namespace Chamedoon.Domin.Configs;

public class MelipayamakConfig
{
    public const string SectionName = "Melipayamak";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://rest.payamak-panel.com/api/SendSMS/SendSMS";

    public string OtpApiBaseAddress { get; set; } = "https://console.melipayamak.com";
    public string OtpTemplateCode { get; set; } = string.Empty;
}
