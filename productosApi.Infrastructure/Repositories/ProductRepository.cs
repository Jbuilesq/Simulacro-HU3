using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;
using productosApi.Infrastructure.Data;

namespace productosApi.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
        
    }
}