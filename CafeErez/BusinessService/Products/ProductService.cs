using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.Model.Product;
using DAL.Connectivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessService.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IServiceWrapper<Product>> AddNewProduct(Product NewProduct)
        {
            var productPrice = await _db.Products.AddAsync(NewProduct);
            await _db.SaveChangesAsync();
            return await ServiceWrapper<Product>.SuccessAsync(productPrice.Entity);
        }

        public async Task<IServiceWrapper<Product>> GetProductPrice(string ProductId)
        {
            var productPrice = await _db.Products.Select(x =>x).Where( x=>x.ProductId.Equals(ProductId)).FirstAsync();
            return await ServiceWrapper<Product>.SuccessAsync(productPrice);
        }
    }
}
