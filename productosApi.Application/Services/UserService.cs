using System.Security.Cryptography;
using System.Text;
using productosApi.Application.Dto;
using productosApi.Application.Interfaces;
using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;

namespace productosApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;


    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<User> Create(RegisterRequestDto register)
    {
        // Validamos si el usuario ya existe
        var existingUser = await _repository.GetByUsernameAsync(register.Username);
        if (existingUser != null)
            throw new Exception("El usuario ya existe.");

        // Creamos un nuevo usuario
        var user = new User
        {
            Username = register.Username,
            Email = register.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password), // Encriptamos la contraseña
            Role = register.Role == "Admin" ? Role.Admin : Role.User  // Aseguramos que se asigna un rol adecuado
        };

        // Guardamos el usuario en la base de datos
        await _repository.AddAsync(user);
        return user;
    }
    
    public async Task<User> Update(User user)
    {
        var existing = await _repository.FindByIdAsync(user.Id);
        if (existing == null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Role = user.Role;

        return await _repository.UpdateAsync(existing);
    }

    public async Task Delete(int id)
    {
        var existing = await _repository.FindByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        await _repository.DeleteAsync(existing);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
    
    
}