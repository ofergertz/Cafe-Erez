using BusinessService.Roles;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.Model.Product;
using DAL.Connectivity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<ProductService> _localizer;

        public ProductService(ApplicationDbContext db, IStringLocalizer<ProductService> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        public async Task<IServiceWrapper<Product>> AddOrEditProduct(Product NewProduct)
        {
            EntityEntry<Product> Product = null;
            var existingProduct = await _db.Products.FindAsync(NewProduct.ProductId);

            if (existingProduct == null)
                Product = await _db.Products.AddAsync(NewProduct);
            else
            {
                existingProduct.ProductId = NewProduct.ProductId;
                existingProduct.ProductDescription = NewProduct.ProductDescription;
                existingProduct.Price = NewProduct.Price;
                existingProduct.AdditionalIdentifier = NewProduct.AdditionalIdentifier;
            }

            await _db.SaveChangesAsync();
            return await ServiceWrapper<Product>.SuccessAsync(Product.Entity);
        }

        public async Task<IServiceWrapper<string>> DeleteAsync(string ProductId)
        {
            var existingProduct = await _db.Products.FindAsync(ProductId);
            if (existingProduct != null)
            {
                _db.Products.Remove(existingProduct);
                await _db.SaveChangesAsync();
                return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["{0} Was deleted"], existingProduct.ProductDescription));
            }
                
            return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Produt {0} does not exist."], ProductId));
        }

        public async Task<IServiceWrapper<List<Product>>> GetAllProducts()
        {
            var products = await _db.Products.Select(p => p).ToListAsync();
            return await ServiceWrapper<List<Product>>.SuccessAsync(products);
        }

        public async Task<IServiceWrapper<Product>> GetProductPrice(string ProductId)
        {
            var productPrice = await _db.Products.Select(x =>x).Where( x=>x.ProductId.Equals(ProductId)).FirstAsync();
            return await ServiceWrapper<Product>.SuccessAsync(productPrice);
        }
    }
}
