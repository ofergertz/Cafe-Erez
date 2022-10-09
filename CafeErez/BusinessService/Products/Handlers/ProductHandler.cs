using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessService.Products.Handlers
{
    public class ProductHandler : IProductHandler
    {
        private readonly HttpClient _httpClient;

        public ProductHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper<Product>> AddNewProduct(Product Product)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.Products.AddProduct, Product);
            return await response.ToResult<Product>();
        }

        public async Task<IServiceWrapper<Product>> PriceQuery(string ProductId)
        {
            var response = await _httpClient.GetAsync(string.Format(Constants.Products.PriceQuery(ProductId)));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return await response.ToResult<Product>();
        }
    }
}
