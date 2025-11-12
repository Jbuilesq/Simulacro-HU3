using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using productosApi.Application.Dto;
using productosApi.Application.Interfaces;
using productosApi.Application.Services;

namespace productosApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IUserService _userService;

    public AuthController(AuthService authService, IUserService userService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            return BadRequest(new { message = "Datos de login no proporcionados correctamente" });

        var token = await _authService.Autenticate(loginRequest.Username, loginRequest.Password);
        if (token == null)
            return Unauthorized(new { message = "Usuario o contraseña incorrecta" });

        return Ok(new { token });
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto register)
    {
        if (register == null || string.IsNullOrEmpty(register.Username) || string.IsNullOrEmpty(register.Password) || string.IsNullOrEmpty(register.Email))
            return BadRequest(new { message = "Los datos de registro son incompletos o incorrectos." });

        try
        {
            var user = await _userService.Create(register);  // Aquí corregimos la llamada al servicio adecuado
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
