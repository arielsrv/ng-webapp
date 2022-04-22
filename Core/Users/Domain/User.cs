namespace Core.Users.Domain;

public class User
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
}