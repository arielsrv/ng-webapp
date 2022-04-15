using System.Reactive.Threading.Tasks;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Microsoft.AspNetCore.Mvc;

namespace Core.Users.Infrastructure;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserQuery _userQuery;

    public UserController(IUserQuery userQuery)
    {
        this._userQuery = userQuery;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetUser([FromRoute] long id)
    {
        return new OkObjectResult(await this._userQuery.GetById(id).ToTask());
    }
}