using BusinessComponents.Identity.Roles;
using BusinessComponents.Identity.Users;
using BusinessService.Customer;
using BusinessService.Customer.Handlers;
using BusinessService.Products.Handlers;
using BusinessService.Reports;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Authentication;
using CafeErez.Shared.BusinessService.Customer.Handlers;
using CafeErez.Shared.BusinessService.Products;
using CafeErez.Shared.Model.Identity;
using CafeErez.Shared.Model.Infrastructure;
using CafeErez.Shared.Model.Users;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.IOC
{
    public static class ClientRegistration
    {
        public static void RegisterClientComponents(this IServiceCollection builder)
        {
            builder.AddTransient<IAuthenticationManager, AuthenticationManager>();
            builder.AddTransient<ICustomerHandler, CustomerHandler>();
            builder.AddTransient<IProductHandler, ProductHandler>();
            builder.AddTransient<IRoleManager, RolesManager>();
            builder.AddTransient<IUserManager, UserManager>();
            builder.AddScoped<CustomAuthenticationStateProvider>();
            builder.AddSingleton<PdfService>();
            builder.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }
    }
}
