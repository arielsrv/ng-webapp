using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using Core.Shared.Errors;
using Core.Users.Domain;
using Core.Users.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Unit.Users.Infrastructure;

public class UserHttpRepositoryTest
{
    private readonly Mock<HttpClient> httpClient;
    private readonly UserHttpRepository userHttpRepository;

    public UserHttpRepositoryTest()
    {
        this.httpClient = new Mock<HttpClient>();
        Mock<ILogger<UserHttpRepository>> logger = new();
        this.userHttpRepository = new UserHttpRepository(this.httpClient.Object, logger.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHttpResponse());

        User actual = this.userHttpRepository.GetUser(1L).Wait();

        Assert.NotNull(actual);
        Assert.Equal(1L, actual.Id);
        Assert.Equal("John Doe", actual.Name);
        Assert.Equal("john@doe.com", actual.Email);
    }

    [Fact]
    public void Get_User_Not_Found()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHttpResponse(HttpStatusCode.NotFound));

        User? actual = this.userHttpRepository.GetUser(1L).Wait();
        
        Assert.Null(actual);
    }

    [Fact]
    public void Get_User_Bad_Request()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHttpResponse(HttpStatusCode.BadRequest));

        ApiBadRequestException actual = Assert.Throws<ApiBadRequestException>(() =>
        {
            this.userHttpRepository.GetUser(1L).Wait();
        });

        Assert.NotNull(actual);
        Assert.Equal("Request failed with uri https://gorest.co.in/public/v2/users/1. Status code: 400.",
            actual.Message);
    }

    [Fact]
    public void Get_User_Internal_Server_Error()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHttpResponse(HttpStatusCode.InternalServerError));

        ApiException actual = Assert.Throws<ApiException>(() => { this.userHttpRepository.GetUser(1L).Wait(); });

        Assert.NotNull(actual);
        Assert.Equal("Internal Server Error", actual.Message);
    }

    private static HttpResponseMessage GetHttpResponse(HttpStatusCode httpStatusCode)
    {
        return new HttpResponseMessage
        {
            Content = new StringContent("{\"message\":\"Error\"}"),
            StatusCode = httpStatusCode
        };
    }

    [Fact]
    public void Get_All_Users()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetHttpResponseList());

        IEnumerable<User> actual = this.userHttpRepository.GetUsers()
            .Wait()
            .ToList();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Single(actual);
        Assert.Equal(1, actual.First().Id);
        Assert.Equal("John Doe", actual.First().Name);
        Assert.Equal("john@doe.com", actual.First().Email);
    }

    private static HttpResponseMessage GetHttpResponseList()
    {
        return new HttpResponseMessage
        {
            Content = new StringContent("[{\"id\":1,\"name\":\"John Doe\",\"email\":\"john@doe.com\"}]")
        };
    }

    private static HttpResponseMessage GetHttpResponse()
    {
        return new HttpResponseMessage
        {
            Content = new StringContent("{\"id\":1,\"name\":\"John Doe\",\"email\":\"john@doe.com\"}")
        };
    }
}