
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Customer;
using MudBlazor;
using System.Net.Http;

namespace CafeErez.Client.Pages
{
    public partial class Customers
    {
        private string searchString = "";
        private Customer customer = new Customer();
        private List<Customer> customers = new List<Customer>();
        protected override async Task OnInitializedAsync()
        {
            customers = await GetCustomers();
        }

        private async Task<List<Customer>> GetCustomers()
        {
            var customersResult = await _customerHandler.GetCustomers();
            return customersResult.Data;
        }

        private bool Search(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (customer.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                || customer.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                || customer.PhoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            _snackBar.Add("Customer not found.", Severity.Error);
            return false;
        }

        private async Task Save(Customer customer)
        {
            var response = await _customerHandler.SaveCustomer(customer);
            if (response.Data == null)
            {
                _snackBar.Add("Error in saving customer.", Severity.Error);
                return;
            }

            _snackBar.Add("Customer Saved.", Severity.Success);
            customers = await GetCustomers();
            StateHasChanged();
        }

        private string GetTotals(Customer customer)
        {
            var paisDebts = int.Parse(customer.CustomerDebts.PaisDebts);
            var winnerDebts = int.Parse(customer.CustomerDebts.WinnerDebts);
            var storeDebts = int.Parse(customer.CustomerDebts.StoreDebts);
            
            var totals = paisDebts + winnerDebts + storeDebts;
            return totals.ToString();
        }

        private async Task Edit(int id)
        {
            customer = customers.FirstOrDefault(c => c.Id == id);
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.EditCustomerDetails.UpdatedCustomer), customer }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.EditCustomerDetails>("Edit", parameters, options);
            var result = await dialog.Result;
            await UpdateCustomer(result.Data as Customer);
        }

        private async Task UpdateCustomer(Customer? customer)
        {
            await _customerHandler.UpdateCustomer(customer);
            _snackBar.Add("Customer Updated.", Severity.Success);
            customers = await GetCustomers();
            StateHasChanged();
        }

        private async Task AddCustomer()
        {
           // string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.AddNewCustomerForDebts.NewCustomer), new Customer() }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.AddNewCustomerForDebts>("Add", parameters, options);
            var result = await dialog.Result;
            await Save(result.Data as Customer);
        }

        private async Task DeleteCustomer(int customerId)
        {
            await _customerHandler.DeleteCustomer(customerId);
            customers = await GetCustomers();
            StateHasChanged();
        }

        private async Task Delete(int id)
        {
            var customerToDelete = await _customerHandler.GetCustomerById(id);
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteCustomerFromTable.DeletedCustomer), $"Delete {customerToDelete.Data.FirstName} {customerToDelete.Data.LastName} from the list?" }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteCustomerFromTable>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await DeleteCustomer(id);
                _snackBar.Add("Customer Deleted.", Severity.Success);
                return;
            }

            _snackBar.Add("Customer Not Deleted.", Severity.Error);
      
        }
    }
}
