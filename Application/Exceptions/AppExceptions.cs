namespace Application.Exceptions;

using System;
using System.Collections.Generic;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}

public class ValidationException : Exception
{
    public List<string> Errors { get; set; }

    public ValidationException(List<string> errors)
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }
}
