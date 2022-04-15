using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Web;
using Xunit;

namespace Test.Integration.Users.Infrastructure;

public class UserControllerTest
{
    private readonly HttpClient httpClient;
    private readonly Mock<IUserQuery> userQuery;

    public UserControllerTest()
    {
        this.userQuery = new Mock<IUserQuery>();
        WebApplicationFactory<App> application = new WebApplicationFactory<App>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(this.userQuery.Object); });
            });
        this.httpClient = application.CreateClient();
    }

    [Fact]
    public async void Get_Ok()
    {
        this.userQuery
            .Setup(query => query.GetById(1L))
            .Returns(GetUserDto());

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users/1");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        UserDto actual = JsonConvert.DeserializeObject<UserDto>(responseString);
        Assert.NotNull(actual);
        Assert.Equal(1L, actual.Id);
        Assert.Equal("John Doe", actual.Name);
        Assert.Equal("john@doe.com", actual.Email);
    }

    [Fact]
    public async void Get_All_Ok()
    {
        this.userQuery
            .Setup(query => query.GetAll())
            .Returns(GetUserDtoList());

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        IEnumerable<UserDto> actual = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(responseString)
            .ToList();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Single(actual);
        Assert.Equal(1, actual.First().Id);
        Assert.Equal("John Doe", actual.First().Name);
        Assert.Equal("john@doe.com", actual.First().Email);
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