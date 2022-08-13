using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Customer;
using Microsoft.AspNetCore.Mvc;

namespace CafeErez.Server.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("GetCustomers")]
        public async Task<ActionResult> GetCustomers()
        {
            var response = await _customerService.GetCustomersAsync();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetCustomerById(string id)
        {
            var response = await _customerService.GetCustomerById(int.Parse(id));
            return Ok(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult> SaveCustomer(Customer Customer)
        {
            var response = await _customerService.SaveCustomer(Customer);
            return Ok(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> UpdateCustomer(Customer Customer)
        {
            var response = await _customerService.UpdateCustomer(Customer);
            return Ok(response);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteCustomer([FromBody] int customerId)
        {
            var response = await _customerService.DeleteCustomer(customerId);
            return Ok(response);
        }
    }
}
