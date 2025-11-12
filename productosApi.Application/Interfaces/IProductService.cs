using productosApi.Domain.Entities;

namespace productosApi.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product> Create(Product product);
    Task<Product> Update(Product product);
    Task Delete(int id);
}