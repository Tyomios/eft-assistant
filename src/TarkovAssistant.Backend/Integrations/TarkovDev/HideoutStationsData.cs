using System.Text.Json.Serialization;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class HideoutStationsData
{
    /// <summary>Gets the hideout stations returned by the query.</summary>
    [JsonPropertyName("hideoutStations")]
    public IReadOnlyList<HideoutStationDto> HideoutStations { get; init; } = [];
}
