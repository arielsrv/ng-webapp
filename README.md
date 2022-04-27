[![.NET](https://github.com/arielsrv/ng-webapp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/arielsrv/ng-webapp/actions/workflows/dotnet.yml)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/arielsrv/294599cbadb71f3ed834d6904a7c87fd/raw/code-coverage.json)

# ng-webapp

Illustrates how to use Rx in an dotnet core api.

### Controller

```csharp
[HttpGet("multi-get")]
public async Task<IActionResult> GetUsers([FromQuery] string ids)
{
    IEnumerable<long> request = ids.ToEnumerable();
    return await TaskExecutor.ExecuteAsync(this.userQuery.GetById(request));
}
```

### Service

```csharp
public IObservable<IEnumerable<MultiGetDto<UserDto>>> GetById(IEnumerable<long> elements)
{
    return elements
        .Select(id => this.GetById(id)
            .Map(userDto => userDto != null
                ? MultiGetDto<UserDto>.CreateOk(userDto)
                : MultiGetDto<UserDto>.CreateNotFound(new UserDto
                {
                    Id = id
                })))
        .Merge(10, Scheduler.Default)
        .ToList()
        .Map(multiGetDtos => multiGetDtos
            .OrderBy(multiGetDto => multiGetDto.Body!.Id)
            .AsEnumerable());
}
```