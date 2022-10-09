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

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult> AddProduct(Product NewProduct)
        {
            var response = await _productService.AddNewProduct(NewProduct);
            return Ok(response);
        }
    }
}
