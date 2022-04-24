namespace Core.Shared;

public class MultiGetDto<T>
{
    public int Code { get; set; }
    public T? Body { get; init; }
}