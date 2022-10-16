using CafeErez.Shared.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Products
{
    public interface IProductHandler
    {
        Task<IServiceWrapper<Product>> PriceQuery(string ProductId); 
        Task<IServiceWrapper<Product>> AddNewProduct(Product Product); 
        Task<IServiceWrapper<List<Product>>> GetAllProducts(); 
        Task<IServiceWrapper<string>> DeleteAsync(string ProductId); 
    }
}
