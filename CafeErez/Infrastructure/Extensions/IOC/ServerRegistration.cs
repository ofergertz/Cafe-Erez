﻿using BusinessComponents.DatabaseSeeder;
using BusinessComponents.Identity;
using BusinessService;
using BusinessService.Customer;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using CafeErez.Shared.Model.Seeder;
using Infrastructure.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.IOC
{
    public static class ServerRegistration
    {
        public static void RegisterServerComponents(this IServiceCollection builder)
        {
            builder.AddTransient<IServiceWrapper, ServiceWrapper>();
            builder.AddTransient<IIdentityService, IdentityService>();
            builder.AddTransient<IUsersHandler, UsersHandler>();
            builder.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
            builder.AddTransient<IMapper<CafeErez.Shared.Model.Identity.RegisterRequest, User>, UserMapper>();
            builder.AddTransient<ICustomerService, CustomerService>();
        }
    }
}