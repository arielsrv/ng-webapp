using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Core.Shared.Errors;
using Core.Shared.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Test.Unit.Shared;

public class TaskExecutorTest
{
    [Fact]
    public async Task OkResult()
    {
        IObservable<int> observable = Observable.Return(1);

        IActionResult actual = await TaskExecutor.ExecuteAsync(observable);

        Assert.NotNull(actual);
        Assert.IsType<OkObjectResult>(actual);
    }

    [Fact]
    public async Task NotFound()
    {
        IObservable<string?> observable = Observable.Return(default(string));

        await Assert.ThrowsAsync<ApiNotFoundException>(async () =>
        {
            await TaskExecutor.ExecuteAsync(observable);
        });
    }
}