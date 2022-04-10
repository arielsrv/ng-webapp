using System;
using System.Reactive.Linq;
using Core.Users.Application;
using Core.Users.Domain;
using Moq;
using Xunit;

namespace Test.Unit.Users.Application;

public class GetUserTest
{
    private readonly Mock<IUserRepository> userRepository;
    private readonly GetUser query;

    public GetUserTest()
    {
        this.userRepository = new Mock<IUserRepository>();
        this.query = new GetUser(this.userRepository.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.userRepository
            .Setup(repository => repository.GetUser(1L))
            .Returns(GetUserResponse());

        UserDto actual = this.query.ById(1L).Wait();

        Assert.NotNull(actual);
        Assert.Equal(1L, actual.Id);
        Assert.Equal("John Doe", actual.Name);
        Assert.Equal("john@doe.com", actual.Email);
    }

    private static IObservable<User> GetUserResponse()
    {
        User user = new()
        {
            Id = 1L,
            Name = "John Doe",
            Email = "john@doe.com"
        };

        return Observable.Return(user);
    }
}