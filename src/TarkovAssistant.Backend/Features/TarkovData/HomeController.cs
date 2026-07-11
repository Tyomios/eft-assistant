using Microsoft.AspNetCore.Mvc;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides the browser entry point for local API exploration.
/// </summary>
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class HomeController : ControllerBase
{
    /// <summary>Redirects the application root to the Scalar API reference.</summary>
    /// <returns>A redirect to the Scalar API reference.</returns>
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Redirect("/scalar/v1");
    }
}
