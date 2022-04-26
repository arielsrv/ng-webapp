using System.Reactive.Threading.Tasks;
using Core.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Core.Shared.Tasks;

public static class TaskExecutor
{
    public static async Task<IActionResult> ExecuteAsync<T>(IObservable<T> observable)
    {
        T result = await observable.ToTask();

        if (result == null)
        {
            throw new ApiNotFoundException("Resource not found. ");
        }

        return new OkObjectResult(result);
    }
}