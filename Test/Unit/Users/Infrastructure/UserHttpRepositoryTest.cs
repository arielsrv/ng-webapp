using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using Core.Users.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Unit.Users.Infrastructure;

public class UserHttpRepositoryTest
{
    private readonly Mock<HttpClient> httpClient;
    private readonly Mock<ILogger<UserHttpRepository>> logger;
    private readonly UserHttpRepository userHttpRepository;

    public UserHttpRepositoryTest()
    {
        this.httpClient = new Mock<HttpClient>();
        this.logger = new Mock<ILogger<UserHttpRepository>>();
        this.userHttpRepository = new UserHttpRepository(this.httpClient.Object, this.logger.Object);
    }

    [Fact]
    public void Get_User()
    {
        this.httpClient
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetResponse());

        UserResponse actual = this.userHttpRepository.GetUser(1L).Wait();

        Assert.NotNull(actual);
        Assert.Equal(1L, actual.Id);
        Assert.Equal("John Doe", actual.Name);
        Assert.Equal("john@doe.com", actual.Email);
    }

    private static HttpResponseMessage GetResponse()
    {
        return new HttpResponseMessage
        {
            Content = new StringContent("{\"id\":1,\"name\":\"John Doe\",\"email\":\"john@doe.com\"}")
        };
    }
}