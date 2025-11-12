using Microsoft.EntityFrameworkCore;
using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;
using productosApi.Infrastructure.Data;

namespace productosApi.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository 
{
    public UserRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

}