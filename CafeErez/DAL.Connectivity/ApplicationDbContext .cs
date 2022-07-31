using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.Infrastructure.ConfigurationRepository;
using CafeErez.Shared.Model.Customer;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Connectivity
{
    public class ApplicationDbContext : IdentityDbContext<User, AppRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public ApplicationDbContext()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerDebts> CustomerDebts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>()
            .HasOne(a => a.CustomerDebts)
            .WithOne(a => a.Customer)
            .HasForeignKey<CustomerDebts>(c => c.Id);

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users", "dbo");

            });
            builder.Entity<Customer>(entity =>
            {
                entity.ToTable(name: "Customers", "dbo");

            });
            builder.Entity<CustomerDebts>(entity =>
            {
                entity.ToTable(name: "CustomerDebts", "dbo");

            });
        }
         public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = new ConfigurationRepository().
                GetValueByKey("ConnectionStrings", "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}