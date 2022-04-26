namespace Core.Shared.Errors;

public class ErrorModel
{
    public ErrorModel(int code, string type, string message)
    {
        this.Code = code;
        this.Type = type;
        this.Message = message;
    }

    public int Code { get; }
    public string Type { get; }
    public string Message { get; }
}