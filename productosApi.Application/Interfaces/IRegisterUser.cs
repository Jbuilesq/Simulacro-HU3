using productosApi.Domain.Entities;

namespace productosApi.Application.Interfaces;

public interface IRegisterUser
{
    Task<User> RegisterUser(User user, string password);
}   