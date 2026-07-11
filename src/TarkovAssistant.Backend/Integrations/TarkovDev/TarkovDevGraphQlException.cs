namespace TarkovAssistant.Backend.Integrations.TarkovDev;

/// <summary>
/// Represents one or more GraphQL execution errors returned by Tarkov.dev.
/// </summary>
public sealed class TarkovDevGraphQlException : TarkovDevException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevGraphQlException"/> class.
    /// </summary>
    public TarkovDevGraphQlException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevGraphQlException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TarkovDevGraphQlException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevGraphQlException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public TarkovDevGraphQlException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevGraphQlException"/> class.
    /// </summary>
    /// <param name="errors">The GraphQL error messages.</param>
    public TarkovDevGraphQlException(IReadOnlyList<string> errors)
        : base($"Tarkov.dev returned GraphQL errors: {string.Join("; ", errors ?? throw new ArgumentNullException(nameof(errors)))}")
    {
        Errors = Array.AsReadOnly([.. errors]);
    }

    /// <summary>Gets the GraphQL error messages.</summary>
    public IReadOnlyList<string> Errors { get; } = [];
}
