using System.Reactive.Threading.Tasks;
using Core.Shared.Errors;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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
        return new OkObjectResult(await this.userQuery.GetAll().ToTask());
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetUser([FromRoute] long id)
    {
        return new OkObjectResult(await this.userQuery.GetById(id).ToTask());
    }

    [HttpGet("multi-get")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetUsers([FromQuery] string ids)
    {
        IEnumerable<long> request = ids.ToEnumerable();
        return new OkObjectResult(await this.userQuery.GetById(request).ToTask());
    }
}