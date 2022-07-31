using BusinessService.Authentication;
using BusinessService.Customer;
using BusinessService.Customer.Handlers;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Authentication;
using CafeErez.Shared.BusinessService.Customer.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.IOC
{
    public static class ClientRegistration
    {
        public static void RegisterClientComponents(this IServiceCollection builder)
        {
            builder.AddTransient<IAuthenticationManager, AuthenticationManager>();
            builder.AddTransient<ICustomerHandler, CustomerHandler>();
        }
    }
}
