using CafeErez.Shared.Model.Customer;

namespace CafeErez.Shared.BusinessService.Customer.Handlers
{
    public interface ICustomerHandler
    {
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> GetCustomerById(int Id);
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> UpdateCustomer(CafeErez.Shared.Model.Customer.Customer Customer);
        Task<IServiceWrapper<List<CafeErez.Shared.Model.Customer.Customer>>> GetCustomers();
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> SaveCustomer(CafeErez.Shared.Model.Customer.Customer Customer);
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> DeleteCustomer(int CustomerId);
        Task<IServiceWrapper<CafeErez.Shared.Model.Customer.Customer>> SaveCustomerDiary(CafeErez.Shared.Model.Customer.Customer Customer, DateTime ActionDate);

    }
}
