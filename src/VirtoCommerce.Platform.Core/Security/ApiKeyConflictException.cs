using System;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// A save would give an API key a value that another key already has. The message names neither the value, its
/// hash nor the other key's owner, so it can be shown to the client that sent the value.
/// </summary>
public class ApiKeyConflictException : PlatformException
{
    public const string DefaultMessage = "This API key cannot be used. Send an empty value to have the platform generate one.";

    public ApiKeyConflictException()
        : base(DefaultMessage)
    {
    }

    public ApiKeyConflictException(Exception innerException)
        : base(DefaultMessage, innerException)
    {
    }
}
