using productosApi.Application.Interfaces;
using productosApi.Domain.Entities;
using productosApi.Domain.Interfaces;

namespace productosApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _productRepository.FindByIdAsync(id);
        }

        public async Task<Product> Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");

            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");

            if (product.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> Update(Product product)
        {
            var existing = await _productRepository.FindByIdAsync(product.Id);
            if (existing == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

            return await _productRepository.UpdateAsync(existing);
        }

        public async Task Delete(int id)
        {
            var existing = await _productRepository.FindByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            await _productRepository.DeleteAsync(existing);
        }
    }
}
