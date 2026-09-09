namespace TaskManager.WebApi.Controllers;
using TaskManager.WebApi.Services;
using TaskManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// NOTE: intentionally unprotected for now (no auth middleware yet) - real access control
// (restricting this to admins only) lands in the upcoming authentication work.
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string? search)
    {
        var users = await _userService.SearchUsersAsync(search);
        return Ok(users);
    }
}
