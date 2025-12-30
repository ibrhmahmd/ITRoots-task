using System;

namespace StudentRegistrationSystem.Core.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a duplicate entity
/// </summary>
public class DuplicateException : BusinessException
{
    public DuplicateException(string message) : base(message, "DUPLICATE_ENTITY")
    {
    }

    public DuplicateException(string message, Exception innerException) 
        : base(message, "DUPLICATE_ENTITY", innerException)
    {
    }
}
