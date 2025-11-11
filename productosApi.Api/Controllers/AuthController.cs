using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using productosApi.Application.Dto;
using productosApi.Application.Services;

namespace productosApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;
    
    public  AuthController(AuthService authService,  UserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var token = await _authService.Autenticate(loginRequest.Username, loginRequest.Password);
        if (token == null)
        {
            return Unauthorized(new{message = "Username or password is incorrect"});
        }
        return Ok(new{token});
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto register)
    {
        try
        {
            var user = await _userService.Create(register);
            return Ok(new
            {
                message = "Usuario registrado correctamente",
                user = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
