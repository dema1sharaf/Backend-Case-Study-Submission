using Catalog.Model;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _db;

        public ProductRepository(ProductDbContext db)
        {
            this._db = db;
        }

        public async Task<List<Product>> GetProducts() => await _db.Product.ToListAsync();

        public async Task<bool> PutProduct(Product product)
        {
            _db.Product.Update(product);
            return _db.SaveChanges() > 0;
        }

        public async Task<Product> AddProduct(Product prodcut)
        {
            _db.Product.Add(prodcut);
            _db.SaveChanges();
            return await _db.Product.FirstOrDefaultAsync(x => x.Id == prodcut.Id);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _db.Product.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteProductById(Product product)
        {
            _db.Product.Remove(product);
           return _db.SaveChanges() > 0;

        }

    }
}