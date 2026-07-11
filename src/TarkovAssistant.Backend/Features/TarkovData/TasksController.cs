using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to tasks and their objectives.
/// </summary>
[ApiController]
[Route("api/tarkov/tasks")]
[Produces("application/json")]
public sealed class TasksController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public TasksController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of tasks and their item objectives.</summary>
    /// <param name="parameters">The localization and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of tasks.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<TaskDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<TaskDto>>> GetTasksAsync(
        [FromQuery] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery();
        var tasks = await _client.GetTasksAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, tasks));
    }
}
