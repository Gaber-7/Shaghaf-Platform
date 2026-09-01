namespace Shaghaf.Application.Common.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }
}

public class ValidationException : AppException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.") => Errors = errors;

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}

public class NotFoundException : AppException
{
    public NotFoundException(string name, object key) : base($"{name} '{key}' was not found.")
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message)
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You are not allowed to perform this action.") : base(message)
    {
    }
}

public class AuthenticationException : AppException
{
    public AuthenticationException(string message) : base(message)
    {
    }
}
