using Core.Users.Application;

namespace Core.Shared.Users.Application;

public interface IGetUser : IQuery<long, UserDto>
{
}