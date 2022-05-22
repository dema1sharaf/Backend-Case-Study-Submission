using SharedLayer;
using Catalog.Model;
using CatalogService.Payloads;
using CatalogService.Repository;
using MassTransit;

namespace CatalogService.WebApi.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IBusControl _bus;
       

        public ProductService(IProductRepository productRepository, IBusControl bus)
        {
            _productRepository = productRepository;
            _bus = bus;
         
        }

        public async Task<Product> CreateProduct(ProductPayload product, IFormFileCollection files)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return null;
            }
            string image = "" ;
            if(files != null) {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            image = Convert.ToBase64String(fileBytes);
                            // act on the Base64 data
                        }
                    }
                }
            }
            

            Product newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price ?? 0,
                Cost = product.Cost ?? 0,
                Image = image,
            };
            var result = await _productRepository.AddProduct(newProduct);

            try
            {
                Uri uri = new Uri("rabbitmq://localhost/todoQueue");

                var endPoint = await _bus.GetSendEndpoint(uri);
                var todoo = new MailEvent();
                todoo.info = "New Product Added: " + product.Name + " Price is: " + product.Price;
                await endPoint.Send(todoo);
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            if (id > 0)
            {
                var Product = await _productRepository.GetProductById(id);

                if (Product == null)
                {
                    return false;
                }

                var result = await _productRepository.DeleteProductById(Product);
                return result;

            }
            return false;
        }

        public async  Task<Product> GetProductById(int id)
        {

            var product = await _productRepository.GetProductById(id);
            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return products;
        }

        public async Task<bool> UpdateProduct(int id, ProductPayload product, IFormFileCollection files)
        {
            if (id > 0)
            {
                var oldProduct = await _productRepository.GetProductById(id);

                if (oldProduct == null)
                {
                    return false;
                }
                string image = "";
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                image = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                            }
                        }
                    }
                }
                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price ?? 0;
                oldProduct.Cost = product.Cost ?? 0;
               oldProduct.Image = image;

                var result = await _productRepository.PutProduct(oldProduct);

                return result;

            }
            return false;
        }
    }
}
