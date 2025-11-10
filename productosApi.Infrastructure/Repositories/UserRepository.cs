using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;
using productosApi.Infrastructure.Data;

namespace productosApi.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository 
{
    public UserRepository(AppDbContext context) : base(context)
    {
        
    }
}