using System.Reactive.Observable.Aliases;
using Core.Shared.Http;
using Core.Users.Domain;
using Microsoft.Extensions.Logging;

namespace Core.Users.Infrastructure;

public class UserHttpRepository : Client, IUserRepository
{
    public UserHttpRepository(
        HttpClient httpClient,
        ILogger<UserHttpRepository> logger
    ) : base(httpClient, logger)
    {
    }

    public IObservable<User> GetUser(long id)
    {
        string url = $"https://gorest.co.in/public/v2/users/{id}";
        return this.Get<UserResponse>(url)
            .Map(response =>
            {
                User user = new()
                {
                    Id = response.Id,
                    Name = response.Name,
                    Email = response.Email
                };
                return user;
            });
    }

    public IObservable<IEnumerable<User>> GetUsers()
    {
        const string url = $"https://gorest.co.in/public/v2/users";
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