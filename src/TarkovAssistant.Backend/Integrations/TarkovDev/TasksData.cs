using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class TasksData
{
    /// <summary>Gets the tasks returned by the query.</summary>
    public IReadOnlyList<TaskDto> Tasks { get; init; } = [];
}
