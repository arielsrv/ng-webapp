using System.Reactive.Threading.Tasks;
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

    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return new OkObjectResult(await this.userQuery.GetAll().ToTask());
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetUser([FromRoute] long id)
    {
        return new OkObjectResult(await this.userQuery.GetById(id).ToTask());
    }
}