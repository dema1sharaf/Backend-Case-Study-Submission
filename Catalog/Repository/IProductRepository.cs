using Catalog.Model;

namespace CatalogService.Repository
{
    public interface IProductRepository
    {
        public Task<Product> AddProduct(Product product);
        public Task<List<Product>> GetProducts();
        public Task<bool> PutProduct(Product product);
        public Task<Product> GetProductById(int id);
        public Task<bool> DeleteProductById(Product product);
    }
}