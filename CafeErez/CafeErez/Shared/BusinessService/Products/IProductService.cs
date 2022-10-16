using CafeErez.Shared.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Products
{
    public interface IProductService
    {
        Task<IServiceWrapper<Product>> GetProductPrice(string ProductId);
        Task<IServiceWrapper<string>> DeleteAsync(string ProductId);
        Task<IServiceWrapper<List<Product>>> GetAllProducts();
        Task<IServiceWrapper<Product>> AddOrEditProduct(Product NewProduct);

    }
}
