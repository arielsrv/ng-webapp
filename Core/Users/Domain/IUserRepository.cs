using Core.Users.Infrastructure;

namespace Core.Users.Domain;

public interface IUserRepository
{
    public IObservable<User> GetUser(long id);
}