using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.Model.Product;
using Microsoft.AspNetCore.Mvc;

namespace CafeErez.Server.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("PriceQuery/{id}")]
        public async Task<ActionResult> GetProductPrice(string id)
        {
            var response = await _productService.GetProductPrice(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var response = await _productService.GetAllProducts();
            return Ok(response);

        }

        [HttpDelete("Delete/{productId}")]
        public async Task<IActionResult> DeleteRole(string productId)
        {
            var response = await _productService.DeleteAsync(productId);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult> AddProduct(Product NewProduct)
        {
            var response = await _productService.AddOrEditProduct(NewProduct);
            return Ok(response);
        }
    }
}
