namespace Core.Shared.Errors;

public class ErrorModel
{
    public ErrorModel(int code, string type, string message, string? detail)
    {
        this.Code = code;
        this.Type = type;
        this.Message = message;
        this.Detail = detail;
    }

    public int Code { get; }
    public string Type { get; }
    public string Message { get; }
    public string? Detail { get; }
}