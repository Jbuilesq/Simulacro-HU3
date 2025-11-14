using productosApi.Application.Dto;
using productosApi.Domain.Entities;

namespace productosApi.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User> Create(RegisterRequestDto register);
    Task<User> Update(User user);
    Task Delete(int id);
    
}