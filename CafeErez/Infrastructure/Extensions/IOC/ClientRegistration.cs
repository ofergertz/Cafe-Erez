using BusinessService.Customer;
using BusinessService.Customer.Handlers;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Authentication;
using CafeErez.Shared.BusinessService.Customer.Handlers;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.IOC
{
    public static class ClientRegistration
    {
        public static void RegisterClientComponents(this IServiceCollection builder)
        {
            builder.AddTransient<IAuthenticationManager, AuthenticationManager>();
            builder.AddTransient<ICustomerHandler, CustomerHandler>();
            builder.AddScoped<CustomAuthenticationStateProvider>();
            builder.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }
    }
}
