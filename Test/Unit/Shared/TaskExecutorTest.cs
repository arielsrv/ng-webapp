using System;
using System.Reactive.Linq;
using Core.Shared.Errors;
using Core.Shared.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Test.Unit.Shared;

public class TaskExecutorTest
{
    [Fact]
    public void OkResult()
    {
        IObservable<int> observable = Observable.Return(1);

        IActionResult actual = TaskExecutor.ExecuteAsync(observable)
            .GetAwaiter()
            .GetResult();

        Assert.NotNull(actual);
        Assert.IsType<OkObjectResult>(actual);
    }

    [Fact]
    public void Not_Found()
    {
        IObservable<string?> observable = Observable.Return(default(string));

        Assert.Throws<ApiNotFoundException>(() =>
        {
            TaskExecutor.ExecuteAsync(observable)
                .GetAwaiter()
                .GetResult();
        });
    }
}