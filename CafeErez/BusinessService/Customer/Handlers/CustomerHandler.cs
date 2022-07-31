using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Customer.Handlers;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Customer.Handlers
{
    public class CustomerHandler : ICustomerHandler
    {
        private readonly HttpClient _httpClient;

        public CustomerHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> DeleteCustomer(int CustomerId)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.Customers.DeleteCustomer, CustomerId);
            return await response.ToResult<CafeErez.Shared.Model.Customer.Customer>();
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> GetCustomerById(int Id)
        {
            var response = await _httpClient.GetAsync(Constants.Customers.GetCustomerById(Id));
            return await response.ToResult<CafeErez.Shared.Model.Customer.Customer>();
        }

        public async Task<IServiceWrapper<List<CafeErez.Shared.Model.Customer.Customer>>> GetCustomers()
        {
            var response = await _httpClient.GetAsync(Constants.Customers.GetCustomers);
            return await response.ToResult<List<CafeErez.Shared.Model.Customer.Customer>>();
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> SaveCustomer(CafeErez.Shared.Model.Customer.Customer Customer)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.Customers.SaveCustomer, Customer);
            return await response.ToResult<CafeErez.Shared.Model.Customer.Customer>();
        }

        public async Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> UpdateCustomer(CafeErez.Shared.Model.Customer.Customer Customer)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.Customers.UpdateCustomer, Customer);
            return await response.ToResult<CafeErez.Shared.Model.Customer.Customer>();
        }
    }
}
