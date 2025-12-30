using System;

namespace StudentRegistrationSystem.Core.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found
/// </summary>
public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message, "NOT_FOUND")
    {
    }

    public NotFoundException(string message, Exception innerException) 
        : base(message, "NOT_FOUND", innerException)
    {
    }
}
