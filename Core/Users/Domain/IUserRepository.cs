namespace Core.Users.Domain;

public interface IUserRepository
{
    public IObservable<User> GetUser(long id);
}