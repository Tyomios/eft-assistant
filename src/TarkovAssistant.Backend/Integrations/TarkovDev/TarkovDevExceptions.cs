namespace TarkovAssistant.Backend.Integrations.TarkovDev;

public class TarkovDevException(string message, Exception? innerException = null)
    : Exception(message, innerException);

public sealed class TarkovDevGraphQlException(IReadOnlyList<string> errors)
    : TarkovDevException($"Tarkov.dev returned GraphQL errors: {string.Join("; ", errors)}")
{
    public IReadOnlyList<string> Errors { get; } = errors;
}
