namespace Core.Users.Domain;

public interface IUserRepository
{
    public IObservable<User?> GetUser(long id);
    public IObservable<IEnumerable<User>> GetUsers();
}