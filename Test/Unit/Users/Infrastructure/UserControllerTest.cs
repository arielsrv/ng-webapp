using System;
using System.Collections.Generic;
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
    private readonly UserController userController;
    private readonly Mock<IUserQuery> userQuery;

    public UserControllerTest()
    {
        this.userQuery = new Mock<IUserQuery>();
        this.userController = new UserController(this.userQuery.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.userQuery
            .Setup(query => query.GetById(1L))
            .Returns(GetUserDto());

        IActionResult actual = this.userController
            .GetUser(1L)
            .GetAwaiter()
            .GetResult();

        Assert.NotNull(actual);
        Assert.IsType<OkObjectResult>(actual);

        OkObjectResult result = (OkObjectResult)actual;
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(GetUserDto().Wait(), result.Value);
    }

    [Fact]
    public void Get_All_Users()
    {
        this.userQuery
            .Setup(query => query.GetAll())
            .Returns(GetUserDtoList());

        IActionResult actual = this.userController
            .GetAll()
            .GetAwaiter()
            .GetResult();

        Assert.NotNull(actual);
        Assert.IsType<OkObjectResult>(actual);

        OkObjectResult result = (OkObjectResult)actual;
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    private static IObservable<IEnumerable<UserDto>> GetUserDtoList()
    {
        List<UserDto> userDtoList = new()
        {
            new UserDto
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@doe.com"
            }
        };

        return Observable.Return(userDtoList);
    }

    private static IObservable<UserDto> GetUserDto()
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