using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Core.Shared;
using Core.Shared.Errors;
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
    public async Task Get_Ok()
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
    public async Task Get_All_Ok()
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
        Assert.Equal(1, actual.First().Id);
        Assert.Equal("John Doe", actual.First().Name);
        Assert.Equal("john@doe.com", actual.First().Email);
    }

    [Fact]
    public async void Multi_Get_Ok()
    {
        this.userQuery
            .Setup(query => query.GetById(new List<long> { 1L, 2L }))
            .Returns(GetMultiGetUserDto());

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users/multi-get?ids=1,2");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        IEnumerable<MultiGetDto<UserDto>> actual = JsonConvert
            .DeserializeObject<IEnumerable<MultiGetDto<UserDto>>>(responseString)
            .ToList();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal(2, actual.Count());
        Assert.Contains(actual, userDto => userDto.Body!.Id == 1L);
        Assert.Contains(actual, userDto => userDto.Body!.Id == 2L);
        Assert.Contains(actual, userDto => userDto.Body!.Id == 1L && userDto.Code == 200);
        Assert.Contains(actual, userDto => userDto.Body!.Id == 2L && userDto.Code == 404);
    }
    
    [Fact]
    public async Task Get_All_Internal_Server_Error()
    {
        this.userQuery.Setup(query => query.GetAll())
            .Returns(Observable.Throw<UserDto[]>(new ApiException()));

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        ErrorModel errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        Assert.NotNull(errorModel);
        Assert.Equal(500, errorModel.Code);
        Assert.Equal(nameof(ApiException), errorModel.Type);
    }

    [Fact]
    public async void Get_All_Not_Found()
    {
        this.userQuery.Setup(query => query.GetAll())
            .Returns(Observable.Throw<UserDto[]>(new ApiNotFoundException("not found")));

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        ErrorModel errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        Assert.NotNull(errorModel);
        Assert.Equal(404, errorModel.Code);
        Assert.Equal(nameof(ApiNotFoundException), errorModel.Type);
        Assert.NotNull(errorModel.Message);
    }

    [Fact]
    public async Task Get_All_Bad_Request()
    {
        this.userQuery.Setup(query => query.GetAll())
            .Returns(Observable.Throw<UserDto[]>(new ApiBadRequestException("bad request")));

        HttpResponseMessage httpResponseMessage = await this.httpClient.GetAsync("/users");
        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);

        ErrorModel errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseString);

        Assert.NotNull(errorModel);
        Assert.Equal(400, errorModel.Code);
        Assert.Equal(nameof(ApiBadRequestException), errorModel.Type);
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
            },
            new UserDto
            {
                Id = 2,
                Name = "John Doe",
                Email = "john@doe.com"
            }
        };

        return Observable.Return(userDtoList);
    }

    private static IObservable<IEnumerable<MultiGetDto<UserDto>>> GetMultiGetUserDto()
    {
        List<MultiGetDto<UserDto>> userDtoList = new()
        {
            new MultiGetDto<UserDto>
            {
                Code = 200,
                Body = new UserDto
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@doe.com"
                }
            },
            new MultiGetDto<UserDto>
            {
                Code = 404,
                Body = new UserDto
                {
                    Id = 2
                }
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