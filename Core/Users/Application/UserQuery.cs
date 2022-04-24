using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Observable.Aliases;
using Core.Shared;
using Core.Shared.Users.Application;
using Core.Users.Domain;

namespace Core.Users.Application;

public class UserQuery : IUserQuery
{
    private readonly IUserRepository userRepository;

    public UserQuery(
        IUserRepository userRepository
    )
    {
        this.userRepository = userRepository;
    }

    public IObservable<UserDto> GetById(long id)
    {
        return this.userRepository.GetUser(id)
            .Map(response =>
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

    public IObservable<IEnumerable<UserDto>> GetAll()
    {
        return this.userRepository.GetUsers()
            .Map(result =>
            {
                return result.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                });
            });
    }

    public IObservable<IEnumerable<MultiGetDto<UserDto>>> GetById(IEnumerable<long> elements)
    {
        return Observable.Return(elements
            .Select(element => this.GetById(element)
                .Map(userDto =>
                {
                    MultiGetDto<UserDto> multiGetDto = new()
                    {
                        Code = 200,
                        Body = userDto
                    };
                    return multiGetDto;
                }))
            .Merge(10, Scheduler.Default)
            .ToEnumerable());
    }
}