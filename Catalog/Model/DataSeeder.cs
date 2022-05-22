namespace Catalog.Model
{
    public class DataSeeder
    {
        private readonly ProductDbContext productDbContext;

        public DataSeeder(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }

        public void Seed()
        {
            if (!productDbContext.Product.Any())
            {
                var products = new List<Product>()
                {
                        new Product()
                        {
                            Name = "Dema",
                            Price = 200,
                            Cost  = 150,
                            Image = "56trhhgffgh"
                        },
                        new Product()
                        {
                            Name = "Dema1",
                            Price = 20,
                            Cost  = 15,
                            Image = "56trhhgffgh"
                        }
                };

                productDbContext.Product.AddRange(products);
                productDbContext.SaveChanges();
            }
        }
    }
}
