using Core.Shared;
using Core.Shared.Errors;
using Core.Shared.Tasks;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Microsoft.AspNetCore.Mvc;

namespace Core.Users.Infrastructure;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserQuery userQuery;

    public UserController(IUserQuery userQuery)
    {
        this.userQuery = userQuery;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetAll()
    {
        return await TaskExecutor.ExecuteAsync(this.userQuery.GetAll());
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetUser([FromRoute] long id)
    {
        return await TaskExecutor.ExecuteAsync(this.userQuery.GetById(id));
    }

    [HttpGet("multi-get")]
    [ProducesResponseType(typeof(IEnumerable<MultiGetDto<UserDto>>), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetUsers([FromQuery] string ids)
    {
        IEnumerable<long> request = ids.ToEnumerable();
        return await TaskExecutor.ExecuteAsync(this.userQuery.GetById(request));
    }
}