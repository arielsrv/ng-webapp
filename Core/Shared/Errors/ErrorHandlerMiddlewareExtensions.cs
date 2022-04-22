using Microsoft.AspNetCore.Builder;

namespace Core.Shared.Errors;

public static class ErrorHandlerMiddlewareExtensions
{
    public static void UseErrorHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}