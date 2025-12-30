using System;

namespace StudentRegistrationSystem.Core.Exceptions;

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
