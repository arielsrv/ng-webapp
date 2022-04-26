using System.Reactive.Linq;
using System.Reactive.Observable.Aliases;
using Core.Shared.Http;
using Core.Users.Domain;
using Microsoft.Extensions.Logging;

namespace Core.Users.Infrastructure;

public class UserHttpRepository : Client, IUserRepository
{
    private readonly string urlBase;

    public UserHttpRepository(
        HttpClient httpClient,
        ILogger<UserHttpRepository> logger
    ) : base(httpClient, logger)
    {
        this.urlBase = "https://gorest.co.in/public/v2";
    }

    public IObservable<User?> GetUser(long id)
    {
        string url = $"{this.urlBase}/users/{id}";
        return this.Get<UserResponse>(url)
            .FlatMap(response =>
            {
                if (response == null)
                {
                    return Observable.Return(default(User));
                }

                User user = new()
                {
                    Id = response.Id,
                    Name = response.Name,
                    Email = response.Email
                };
                return Observable.Return(user);
            });
    }

    public IObservable<IEnumerable<User>> GetUsers()
    {
        string url = $"{this.urlBase}/users";
        return this.Get<IEnumerable<UserResponse>>(url)
            .Map(response =>
            {
                return response.Select(userResponse =>
                {
                    User user = new()
                    {
                        Id = userResponse.Id,
                        Name = userResponse.Name,
                        Email = userResponse.Email
                    };
                    return user;
                });
            });
    }
}