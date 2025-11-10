using System.Security.Cryptography;
using System.Text;
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

    public async Task<User> Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("El email es obligatorio.");
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ArgumentException("La contraseña es obligatoria.");

        var existing = await _repository.GetByEmailAsync(user.Email);
        if (existing != null)
            throw new InvalidOperationException("Ya existe un usuario con ese correo.");

        // Hash de la contraseña
        user.PasswordHash = HashPassword(user.PasswordHash);

        return await _repository.AddAsync(user);
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