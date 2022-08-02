using CafeErez.Shared.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService
{
    public interface ICustomerService
    {
        Task<IServiceWrapper<List<CafeErez.Shared.Model.Customer.Customer>>> GetCustomersAsync();
        Task<CafeErez.Shared.Model.Customer.Customer> GetCustomerById(int id);
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> SaveCustomer(CafeErez.Shared.Model.Customer.Customer customer);
        Task<CafeErez.Shared.Model.Customer.Customer> UpdateCustomer(CafeErez.Shared.Model.Customer.Customer customer);
        Task<CafeErez.Shared.Model.Customer.Customer> DeleteCustomer(int id);
    }
}
