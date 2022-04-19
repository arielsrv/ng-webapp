using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Shared.Errors;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception error)
        {
            HttpResponse httpResponse = context.Response;
            httpResponse.ContentType = MediaTypeNames.Application.Json;
            httpResponse.StatusCode = error switch
            {
                ApiBadRequestException => (int)HttpStatusCode.BadRequest,
                ApiNotFoundException => (int)HttpStatusCode.NotFound,

                _ => (int)HttpStatusCode.InternalServerError
            };

            ErrorModel errorModel = new(
                httpResponse.StatusCode,
                error.GetType().Name,
                GetErrorMessage(error),
                error.StackTrace);

            string result = JsonConvert.SerializeObject(errorModel);

            await httpResponse.WriteAsync(result);
        }
    }

    private static string GetErrorMessage(Exception error)
    {
        const string httpClientKey = "HttpClient";

        return error.Data.Count > 0 && error.Data.Contains(httpClientKey)
            ? $"{error.Data[httpClientKey]}. {error.Message}"
            : error.Message;
    }
}