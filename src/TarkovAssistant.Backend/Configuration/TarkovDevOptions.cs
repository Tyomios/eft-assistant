using System.ComponentModel.DataAnnotations;

namespace TarkovAssistant.Backend.Configuration;

/// <summary>
/// Defines the connection settings for the Tarkov.dev API.
/// </summary>
public sealed class TarkovDevOptions
{
    /// <summary>Gets the configuration section name.</summary>
    public const string SectionName = "TarkovDev";

    /// <summary>Gets the Tarkov.dev GraphQL endpoint.</summary>
    [Required]
    public Uri BaseUrl { get; init; } = new("https://api.tarkov.dev/graphql");

    /// <summary>Gets the user-agent header sent to Tarkov.dev.</summary>
    [Required]
    public string UserAgent { get; init; } = "eft-assistant/1.0";
}
