using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reactive.Linq;
using Core.Shared.Errors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Shared.Http;

public class Client : HttpClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<Client> logger;
    public const string HttpClientKey = "HttpClient";

    protected Client(HttpClient httpClient, ILogger<Client> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    protected IObservable<T> Get<T>(string uri)
    {
        return Observable.Create(async (IObserver<T> observer) =>
        {
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, uri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            try
            {
                using HttpResponseMessage httpResponseMessage =
                    await this.httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string message =
                        $"Request failed with uri {uri}. Status code: {(int)httpResponseMessage.StatusCode}.";
                    this.logger.LogError(message);
                    throw httpResponseMessage.StatusCode switch
                    {
                        HttpStatusCode.NotFound => new ApiNotFoundException(message),
                        HttpStatusCode.BadRequest => new ApiBadRequestException(message),
                        _ => new ApiException(httpResponseMessage.ReasonPhrase ?? "Unknown reason")
                    };
                }

                T result = (T)(JsonConvert.DeserializeObject<T>(response) ?? new object());

                observer.OnNext(result);
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                e.Data.Add(HttpClientKey, this.GetType().Name);
                observer.OnError(e);
                throw;
            }
        });
    }
}