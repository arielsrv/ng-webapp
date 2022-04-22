using Core.Users.Application;

namespace Core.Shared.Users.Application;

public interface IUserQuery : IQuery<long, UserDto>
{
    
}