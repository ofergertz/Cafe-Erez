using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Model.Customer;
using CafeErez.Shared.Model.Identity;
using CafeErez.Shared.Model.Product;
using CafeErez.Shared.Model.Seeder;
using DAL.Connectivity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BusinessComponents.DatabaseSeeder
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILookupNormalizer normalizer;

        public DatabaseSeeder(
            UserManager<User> userManager,
            RoleManager<AppRole> roleManager,
            ApplicationDbContext db,
            ILogger<DatabaseSeeder> logger,
            ILookupNormalizer normalizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            this.normalizer = normalizer;
        }

        public void Initialize()
        {
            AddRole();
            AddCustomer();
            AddAdministrator();
            AddBasicUser();
            AddProductWithPrice();
            _db.SaveChanges();
        }

        private void AddProductWithPrice()
        {
            if (_db.Products.Count() > 0)
                return;

            _db.Products.AddAsync(new Product()
            {
                AdditionalIdentifier = "",
                Price = 5.0m,
                ProductDescription = "Mixed Bamba Bisley",
                ProductId = "7290106578692"
            }).GetAwaiter().GetResult();
        }

        private void AddRole()
        {
            Task.Run(async () =>
            {
                var adminRole = new AppRole();
                adminRole.Name = Constants.Roles.AdministratorRole;
                adminRole.Description = "Administrator role with full permissions";
                var adminRoleInDb = await _roleManager.FindByNameAsync(Constants.Roles.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(Constants.Roles.AdministratorRole);
                }

                var basicRole = new AppRole();
                basicRole.Name = Constants.Roles.BasicRole;
                basicRole.Description = "Basic role with default permissions";

                var basicRoleInDb = await _roleManager.FindByNameAsync(Constants.Roles.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    basicRoleInDb = await _roleManager.FindByNameAsync(Constants.Roles.BasicRole);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddCustomer()
        {
            Task.Run(async () =>
            {
                var customer = new Customer("Ofer", "Gertz", "0508948070");
                var customer2 = new Customer("Erez", "Gertz", "0508972804");
                var customer3 = new Customer("Tomer", "Gertz", "0507500317");

                if (_db.Customer.Count() > 0 && _db.Customer.Where(x => x.CustomerId != 0).First() != null)
                    return;

                await _db.Customer.AddRangeAsync(new Customer[] { customer, customer2, customer3 });
            }).GetAwaiter().GetResult();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if User Exists
                var superUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "ofer1g38@gmail.com",
                    UserName = "oferg88",
                    NormalizedUserName = normalizer.NormalizeEmail("oferg88"),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = normalizer.NormalizeEmail("ofer1g38@gmail.com"),
                    PhoneNumber = "0508948070",
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, Constants.Users.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, Constants.Roles.AdministratorRole);
                    if (!result.Succeeded)
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if User Exists
                var basicUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Basic",
                    LastName = "Basic",
                    Email = "miki1g@gmail.com",
                    UserName = "mikig1951",
                    NormalizedUserName = normalizer.NormalizeEmail("mikig1951"),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = normalizer.NormalizeEmail("miki1g@gmail.com"),
                    PhoneNumber = "0507500317",
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, Constants.Users.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(basicUser, Constants.Roles.BasicRole);
                    if (!result.Succeeded)
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
