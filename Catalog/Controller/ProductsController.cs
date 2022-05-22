using Microsoft.AspNetCore.Mvc;
using Catalog.Model;
using CatalogService.WebApi.Services;
using CatalogService.Payloads;

namespace Catalog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProductsController(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            if (!products.Any())
            {
                return NoContent();
            }
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute]int id)
        {

            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(int id,[FromForm] ProductPayload product)
        {
            IFormFileCollection files = null;
            if (_httpContextAccessor.HttpContext != null)
            {
                files = _httpContextAccessor.HttpContext.Request.Form.Files;
            }
            var oldProduct = await _productService.UpdateProduct(id, product, files);
            if (!oldProduct) 
                return BadRequest("Error in update product");
            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
         public async Task<ActionResult<Product>> PostProduct([FromForm] ProductPayload product)

        {
            IFormFileCollection files = null;
            if (_httpContextAccessor.HttpContext != null)
            {
                files = _httpContextAccessor.HttpContext.Request.Form.Files;
            }
            var newProduct = await _productService.CreateProduct(product, files);
            if(newProduct == null)
            {
                return BadRequest("Error in Create product");
            }
            return Ok(newProduct);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            var result = await _productService.DeleteProduct(id);

            if (!result)
            {
                return BadRequest("Error in Delete product");
            }
           return Ok();
        }
    }
}
