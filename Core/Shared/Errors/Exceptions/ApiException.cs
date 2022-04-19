namespace Core.Shared.Errors;

public class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string message) : base(message)
    {
    }
}