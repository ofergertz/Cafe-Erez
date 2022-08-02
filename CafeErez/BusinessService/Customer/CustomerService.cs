using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Customer;
using DAL.Connectivity;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CafeErez.Shared.Model.Customer.Customer> DeleteCustomer(int id)
        {
            var customerToDelete = await GetCustomerById(id);
            _db.Customer.Remove(customerToDelete);
            await _db.SaveChangesAsync();
            return customerToDelete;
        }

        public async Task<CafeErez.Shared.Model.Customer.Customer> GetCustomerById(int id)
        {
            return await _db.Customer.FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task<IServiceWrapper<List<CafeErez.Shared.Model.Customer.Customer>>> GetCustomersAsync()
        {
            var customers = await _db.Customer.Select(x =>x).Include(x=>x.CustomerDebts).ToListAsync();
            return await ServiceWrapper<List<CafeErez.Shared.Model.Customer.Customer>>.SuccessAsync(customers);
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> SaveCustomer(CafeErez.Shared.Model.Customer.Customer customer)
        {
            var customerSaved = _db.Customer.AddAsync(customer).GetAwaiter().GetResult();
            await _db.SaveChangesAsync();
            return await ServiceWrapper<CafeErez.Shared.Model.Customer.Customer>.SuccessAsync(customerSaved.Entity);
        }

        public async Task<CafeErez.Shared.Model.Customer.Customer> UpdateCustomer(CafeErez.Shared.Model.Customer.Customer customer)
        {
            var customerSaved = _db.Customer.Update(customer);
            await _db.SaveChangesAsync();
            return customerSaved.Entity;
        }
    }
}
