namespace Core.Shared.Errors;

public class ApiNotFoundException : Exception
{
    public ApiNotFoundException(string message) : base(message)
    {
    }
}