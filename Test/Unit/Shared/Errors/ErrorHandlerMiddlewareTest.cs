using System.Net;
using System.Threading.Tasks;
using Core.Shared.Errors;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Test.Unit.Shared.Errors;

public class ErrorHandlerMiddlewareTest
{
    private readonly DefaultHttpContext httpContext;

    public ErrorHandlerMiddlewareTest()
    {
        this.httpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task Throws_Not_Found()
    {
        ApiNotFoundException exception = new("not found");

        ErrorHandlerMiddleware errorHandlerMiddleware = new(_ => Task.FromException(exception));
        await errorHandlerMiddleware.InvokeAsync(this.httpContext);

        Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)this.httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task Throws_Internal_Server_Error()
    {
        ApiException exception = new("internal server error");

        ErrorHandlerMiddleware errorHandlerMiddleware = new(_ => Task.FromException(exception));
        await errorHandlerMiddleware.InvokeAsync(this.httpContext);

        Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)this.httpContext.Response.StatusCode);
    }
}