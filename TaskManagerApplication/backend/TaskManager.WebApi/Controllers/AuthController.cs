namespace TaskManager.WebApi.Controllers;
using TaskManager.WebApi.Services;
using TaskManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);
        if (user is null)
        {
            return Conflict("Username is already taken.");
        }
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto);
        if (user is null)
        {
            return Unauthorized("Invalid username or password.");
        }
        return Ok(user);
    }
}
