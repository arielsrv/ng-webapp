namespace Core.Shared.Errors;

public class ApiBadRequestException : Exception
{
    public ApiBadRequestException(string message) : base(message)
    {
    }
}