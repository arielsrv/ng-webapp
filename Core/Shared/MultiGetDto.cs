namespace Core.Shared;

public class MultiGetDto<T>
{
    public int Code { get; set; }
    public T? Body { get; set; }

    private static MultiGetDto<T> CreateNew(int code, T value)
    {
        MultiGetDto<T> multiGetDto = new()
        {
            Body = value,
            Code = code
        };
        return multiGetDto;
    }

    public static MultiGetDto<T> CreateOk(T value)
    {
        return CreateNew(200, value);
    }

    public static MultiGetDto<T> CreateNotFound(T value)
    {
        return CreateNew(404, value);
    }
}