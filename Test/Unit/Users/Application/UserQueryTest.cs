using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Core.Users.Domain;
using Moq;
using Xunit;

namespace Test.Unit.Users.Application;

public class UserQueryTest
{
    private readonly IUserQuery userQuery;
    private readonly Mock<IUserRepository> userRepository;

    public UserQueryTest()
    {
        this.userRepository = new Mock<IUserRepository>();
        this.userQuery = new UserQuery(this.userRepository.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.userRepository
            .Setup(repository => repository.GetUser(1L))
            .Returns(GetUser());

        UserDto actual = this.userQuery.GetById(1L).Wait();

        Assert.NotNull(actual);
        Assert.Equal(1L, actual.Id);
        Assert.Equal("John Doe", actual.Name);
        Assert.Equal("john@doe.com", actual.Email);
    }

    [Fact]
    public void Get_All_Users()
    {
        this.userRepository
            .Setup(repository => repository.GetUsers())
            .Returns(GetUsers());

        IEnumerable<UserDto> actual = this.userQuery.GetAll()
            .Wait()
            .ToList();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Single(actual);
        Assert.Equal(1, actual.First().Id);
        Assert.Equal("John Doe", actual.First().Name);
        Assert.Equal("john@doe.com", actual.First().Email);
    }

    [Fact]
    public void Get_Users_By_Id()
    {
        this.userRepository
            .Setup(repository => repository.GetUser(1L))
            .Returns(Observable.Return(new User { Id = 1L }));

        this.userRepository
            .Setup(repository => repository.GetUser(2L))
            .Returns(Observable.Return(new User { Id = 2L }));

        IEnumerable<UserDto> actual = this.userQuery.GetById(new List<long> { 1L, 2L })
            .Wait()
            .ToList();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal(2, actual.Count());
    }

    private static IObservable<IEnumerable<User>> GetUsers()
    {
        List<User> userDtoList = new()
        {
            new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@doe.com"
            }
        };

        return Observable.Return(userDtoList);
    }

    private static IObservable<User> GetUser()
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