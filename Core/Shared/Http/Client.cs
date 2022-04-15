using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Shared.Http;

public class Client : HttpClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<Client> logger;

    protected Client(HttpClient httpClient, ILogger<Client> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    protected IObservable<T> Get<T>(string requestUri)
    {
        return Observable.Create(async (IObserver<T> observer) =>
        {
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, requestUri);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            try
            {
                using HttpResponseMessage httpResponseMessage =
                    await this.httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
                string response = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    this.logger.LogError(
                        $"Request failed with uri {requestUri}. Status code: {(int)httpResponseMessage.StatusCode}. Raw message: {response}. ");
                    throw httpResponseMessage.StatusCode switch
                    {
                        HttpStatusCode.NotFound => new Exception(response),
                        HttpStatusCode.BadRequest => new Exception(response),
                        _ => new Exception(httpResponseMessage.ReasonPhrase ?? "Unknown reason")
                    };
                }

                T result = (T)(JsonConvert.DeserializeObject<T>(response) ?? new object());

                observer.OnNext(result);
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                e.Data.Add("HttpClient", this.GetType().Name);
                observer.OnError(e);
                throw;
            }
        });
    }
}