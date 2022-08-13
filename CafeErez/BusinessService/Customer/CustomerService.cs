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

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> DeleteCustomer(int id)
        {
            var customerToDelete = await GetCustomerById(id);
            var custoerDeleted = _db.Customer.Remove(customerToDelete.Data);
            await _db.SaveChangesAsync();
            return await ServiceWrapper<CafeErez.Shared.Model.Customer.Customer>.SuccessAsync(custoerDeleted.Entity);
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> GetCustomerById(int id)
        {
            var customer = await _db.Customer.Select(x => x).Where(x => x.CustomerId == id).Include(x=>x.CustomerDebts).FirstAsync();
            return await ServiceWrapper<CafeErez.Shared.Model.Customer.Customer>.SuccessAsync(customer);
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
