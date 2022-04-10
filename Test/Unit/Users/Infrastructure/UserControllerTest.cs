using System;
using System.Reactive.Linq;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Core.Users.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Unit.Users.Infrastructure;

public class UserControllerTest
{
    private readonly Mock<IGetUser> query;
    private readonly UserController userController;

    public UserControllerTest()
    {
        this.query = new Mock<IGetUser>();
        this.userController = new UserController(this.query.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.query
            .Setup(service => service.ById(1L))
            .Returns(GetUser());

        IActionResult actual = this.userController
            .GetUser(1L)
            .GetAwaiter()
            .GetResult();

        Assert.NotNull(actual);
        Assert.IsType<OkObjectResult>(actual);

        OkObjectResult result = (OkObjectResult)actual;
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(GetUser().Wait(), result.Value);
    }

    private static IObservable<UserDto> GetUser()
    {
        UserDto userDto = new()
        {
            Id = 1L,
            Name = "John Doe",
            Email = "john@doe.com"
        };

        return Observable.Return(userDto);
    }
}