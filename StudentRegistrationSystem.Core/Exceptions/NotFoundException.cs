using System;

namespace StudentRegistrationSystem.Core.Exceptions;

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
