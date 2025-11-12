using Microsoft.EntityFrameworkCore;
using productosApi.Domain.Interfaces;
using productosApi.Infrastructure.Data;

namespace productosApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> FindByIdAsync(int id)
    {
        return await  _dbSet.FindAsync(id); 
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SavChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    // public async Task<T> GetByEmailAsync(string email)
            // {
            //     return await _dbContext.Set<T>().FindAsync(email); 
            // }

    // public async Task<T> GetByUsernameAsync(string username)
    // {
    //     return await _dbContext.Set<T>().FindAsync(username);
    // }
}