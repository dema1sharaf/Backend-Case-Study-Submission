using Catalog.Controller;
using Catalog.Model;
using CatalogService.Repository;
using CatalogService.WebApi.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using System.Net;
using CatalogService.Payloads;

namespace XunitTest
{
    public class ProductTesting
    {


        private readonly ProductsController _productsController;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        public ProductTesting()
        {
            _productServiceMock = new Mock<IProductService>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _productsController = new ProductsController(_productServiceMock.Object, _httpContextAccessor.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistProducts()
        {
            var products = CreateProductList();
            _productServiceMock.Setup(c => c.GetProducts()).ReturnsAsync(products);

            ActionResult<List<Product>> result = await _productsController.GetProducts();
            OkObjectResult actual = result.Result as OkObjectResult;
            Assert.NotNull(actual);
            int Ok = (int)HttpStatusCode.OK;
            Assert.Equal(Ok, actual.StatusCode);    
        }



        [Fact]
        public async void GetById_ShouldReturnOK_WhenProductExist()
        {
            var _bus = new Mock<IBusControl>();

            var testProduct = new Product
            {
                Id = 1,
                Name = "Dema3",
                Price = 200,
                Cost = 150,
                Image = "56trhhgffgh"
            };
            _productRepositoryMock.Setup(p => p.GetProductById(1)).ReturnsAsync(testProduct);
            ProductService pro = new ProductService(_productRepositoryMock.Object, _bus.Object);
            var result = await pro.GetProductById(1);
            Assert.True(testProduct.Equals(result));
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _productServiceMock.Setup(c => c.GetProductById(2)).ReturnsAsync((Product)null);

            ActionResult<Product> result = await _productsController.GetProduct(2);
            NotFoundResult actual = result.Result as NotFoundResult;
            Assert.NotNull(actual);
            int NotFound = (int)HttpStatusCode.NotFound;
            Assert.Equal(NotFound, actual.StatusCode);

        }


        [Fact]
        public async void Add_ShouldReturnOk_WhenProductIsAdded()
        {
            var product = CreateProduct();
            ProductPayload pro_payload = new ProductPayload
            {
                Name = product.Name,
                Cost = product.Cost,
                Price = product.Price
            };
            IFormFileCollection files = null;
            _productServiceMock.Setup(c => c.CreateProduct(pro_payload, files)).ReturnsAsync(product);


            ActionResult<Product> result = await _productsController.PostProduct(pro_payload);
            OkObjectResult actual = result.Result as OkObjectResult;
            Assert.NotNull(actual);
            int Ok = (int)HttpStatusCode.OK;
            Assert.Equal(Ok, actual.StatusCode);      
        }

        [Fact]
        public async void Remove_ShouldReturnOk_WhenProductIsRemoved()
        {
            _productServiceMock.Setup(c => c.DeleteProduct(2)).ReturnsAsync(true);

            IActionResult result = await _productsController.DeleteProduct(2);
            var actual = result as OkResult;
            Assert.NotNull(actual);
            int Ok = (int)HttpStatusCode.OK;
            Assert.Equal(Ok, actual.StatusCode);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
           
            _productServiceMock.Setup(c => c.GetProductById(2)).ReturnsAsync((Product)null);

            IActionResult result = await _productsController.DeleteProduct(2);
            var actual = result as BadRequestObjectResult;
            Assert.NotNull(actual);
            int badRequest = (int)HttpStatusCode.BadRequest;
            Assert.Equal(badRequest, actual.StatusCode);
        }




        private List<Product> CreateProductList()
        {
            return new List<Product>()
            {
                new Product()
                {
                Id = 1,
                Name = "Dema3",
                Price = 200,
                Cost = 150,
                Image = "56trhhgffgh"
                },
            };
        }

        private Product CreateProduct()
        {
            return new Product()
            {
                Id = 2,
                Name = "newTest",
                Price = 200,
                Cost = 150,
            };
        }


    }
}