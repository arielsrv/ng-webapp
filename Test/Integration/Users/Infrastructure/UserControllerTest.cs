using System;
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
    private readonly Mock<IGetUser> query;

    public UserControllerTest()
    {
        this.query = new Mock<IGetUser>();
        WebApplicationFactory<App> application = new WebApplicationFactory<App>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(this.query.Object); });
            });
        this.httpClient = application.CreateClient();
    }

    [Fact]
    public async void Get_Ok()
    {
        this.query
            .Setup(service => service.ById(1L))
            .Returns(GetUserDto());

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users/1");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        UserDto userDto = JsonConvert.DeserializeObject<UserDto>(responseString);
        Assert.NotNull(userDto);
        Assert.Equal(1L, userDto.Id);
        Assert.Equal("John Doe", userDto.Name);
        Assert.Equal("john@doe.com", userDto.Email);
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