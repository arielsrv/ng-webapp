using System.Reactive.Threading.Tasks;
using Core.Shared.Users.Application;
using Core.Users.Application;

namespace Core.Users.Infrastructure;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IGetUser getUser;

    public UserController(IGetUser getUser)
    {
        this.getUser = getUser;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetUser([FromRoute] long id)
    {
        return new OkObjectResult(await this.getUser.ById(id).ToTask());
    }
}