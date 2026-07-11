namespace TarkovAssistant.Backend.Integrations.TarkovDev;

/// <summary>
/// Represents a transport or protocol failure while communicating with Tarkov.dev.
/// </summary>
public class TarkovDevException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevException"/> class.
    /// </summary>
    public TarkovDevException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TarkovDevException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public TarkovDevException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
