using System.ComponentModel.DataAnnotations;

namespace TarkovAssistant.Backend.Configuration;

public sealed class TarkovDevOptions
{
    public const string SectionName = "TarkovDev";

    [Required]
    public Uri BaseUrl { get; init; } = new("https://api.tarkov.dev/graphql");

    [Required]
    public string UserAgent { get; init; } = "eft-assistant/1.0";
}
