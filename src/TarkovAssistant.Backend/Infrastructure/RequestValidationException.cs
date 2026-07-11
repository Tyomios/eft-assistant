namespace TarkovAssistant.Backend.Infrastructure;

internal sealed class RequestValidationException : Exception
{
    internal RequestValidationException(string message)
        : base(message)
    {
    }
}
