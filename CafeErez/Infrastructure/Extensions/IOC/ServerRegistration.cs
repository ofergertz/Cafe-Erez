using BusinessComponents.DatabaseSeeder;
using BusinessComponents.Identity;
using BusinessService;
using BusinessService.Customer;
using BusinessService.Products;
using BusinessService.Roles;
using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using CafeErez.Shared.Model.Seeder;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.IOC
{
    public static class ServerRegistration
    {
        public static void RegisterServerComponents(this IServiceCollection builder)
        {
            builder.AddTransient<IServiceWrapper, ServiceWrapper>();
            builder.AddTransient<IIdentityService, IdentityService>();
            builder.AddTransient<IUserService, UserService>();
            builder.AddTransient<IUsersHandler, UsersHandler>();
            builder.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
            builder.AddTransient<IMapper<CafeErez.Shared.Model.Identity.RegisterRequest, User>, UserMapper>();
            builder.AddTransient<IMapper<List<User>, List<UserResponse>>, UserMapper>();
            builder.AddTransient<IMapper<User, UserResponse>, UserMapper>();
            builder.AddTransient<IMapper<List<AppRole>, List<RolesResponse>>, RolesMapper>();
            builder.AddTransient<IMapper<RoleRequest, AppRole>, RolesMapper>();
            builder.AddTransient<ICustomerService, CustomerService>();
            builder.AddTransient<IProductService, ProductService>();
            builder.AddTransient<IRolesService, RolesService>();
        }
    }
}
