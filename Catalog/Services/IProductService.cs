using Catalog.Model;
using CatalogService.Payloads;

namespace CatalogService.WebApi.Services
{
public interface IProductService
{
        public Task<List<Product>> GetProducts();
        public Task<Product> GetProductById(int id);

        public Task<Product> CreateProduct(ProductPayload product,IFormFileCollection files);

        public Task<bool> UpdateProduct(int id, ProductPayload product,IFormFileCollection files);

        public Task<bool> DeleteProduct(int id);


}
}
