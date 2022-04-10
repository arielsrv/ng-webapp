using System.Reactive.Observable.Aliases;
using Core.Shared.Users.Application;
using Core.Users.Domain;

namespace Core.Users.Application;

public class GetUser : IGetUser
{
    private readonly IUserRepository userRepository;

    public GetUser(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public IObservable<UserDto> ById(long id)
    {
        return this.userRepository.GetUser(id).Map(response =>
        {
            UserDto userDto = new()
            {
                Id = response.Id,
                Name = response.Name,
                Email = response.Email
            };
            return userDto;
        });
    }
}