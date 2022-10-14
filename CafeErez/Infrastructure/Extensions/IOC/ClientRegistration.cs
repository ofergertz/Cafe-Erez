using BusinessComponents.Identity.Roles;
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
using Infrastructure.Authentication;
using Infrastructure.Identity;
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
            builder.AddTransient<IApplicationUserManager, ApplicationUserManager>();
            builder.AddTransient<IRoleManager, RolesManager>();
            builder.AddScoped<CustomAuthenticationStateProvider>();
            builder.AddSingleton<PdfService>();
            builder.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }
    }
}
