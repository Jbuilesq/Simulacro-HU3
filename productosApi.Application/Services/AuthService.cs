using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using productosApi.Application.Dto;
using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace productosApi.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> Autenticate(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash)) return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!passwordValid) return null;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keyString = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(keyString))
            throw new Exception("JWT Key configuration missing");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // public async Task<User> UserRegisterAsync(RegisterRequestDto register)
    // {
    //     var existingUser = await _userRepository.GetByUsernameAsync(register.Username);
    //     if (existingUser != null)
    //         throw new Exception("El usuario ya existe.");
    //
    //     var user = new User
    //     {
    //         Username = register.Username,
    //         Email = register.Email,
    //         PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
    //         Role = Role.User
    //     };
    //
    //     await _userRepository.AddAsync(user);
    //     return user;
    // }
}
