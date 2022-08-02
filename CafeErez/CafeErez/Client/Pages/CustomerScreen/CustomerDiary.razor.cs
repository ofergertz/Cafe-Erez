using CafeErez.Shared.Model.Customer;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CafeErez.Client.Pages.CustomerScreen
{
    public partial class CustomerDiary
    {
        private List<Customer> customers = new List<Customer>();
        private List<Customer> existingCustomers = new List<Customer>();
        private string searchString = "";
        private Customer customer = new Customer();


        protected override async Task OnInitializedAsync()
        {
            existingCustomers = await GetCustomers();
        }
        private async Task OnCustomerSelected(Customer value)
        {
            customer = value;
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.AddNewCustomerForDebts.NewCustomer), customer }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.AddNewCustomerForDiaryDebts>(_localizer["Add"], parameters, options);
            var result = await dialog.Result;
            if (result.Cancelled)
                return;

            await UpdateCustomer(result.Data as Customer);
        }

        private string GetCustomerDetails(Customer customer)
        {
            if (customer.CustomerDebts != null && (!string.IsNullOrEmpty(customer.CustomerDebts.PaisDebts) ||
                !string.IsNullOrEmpty(customer.CustomerDebts.WinnerDebts) || !string.IsNullOrEmpty(customer.CustomerDebts.StoreDebts)))
                return customer.FirstName + " " + customer.LastName + " " + _localizer["has own:"] + $" {GetCustomerTotalDebts(customer)}";

            return customer.FirstName + " " + customer.LastName;
        }

        private double GetCustomerTotalDebts(Customer customer)
        {
            return Convert.ToDouble(customer.CustomerDebts.PaisDebts) +
                Convert.ToDouble(customer.CustomerDebts.WinnerDebts) +
                Convert.ToDouble(customer.CustomerDebts.StoreDebts);
        }

        private async Task AddAction()
        {
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.AddNewCustomerForDebts.NewCustomer), new Customer() }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.AddNewCustomerForDiaryDebts>(_localizer["Add"], parameters, options);
            var result = await dialog.Result;
            if (result.Cancelled)
                return;
            await AddCustomer(result.Data as Customer);
        }

        private async Task UpdateCustomer(Customer customer)
        {
            await _customerHandler.UpdateCustomer(customer);
            customers.Add(customer);
            _snackBar.Add(_localizer["Customer Updated."], Severity.Success);
            StateHasChanged();
        }
        private async Task AddCustomer(Customer customer)
        {
            var response = await _customerHandler.SaveCustomer(customer);
            if (response.Data == null)
            {
                _snackBar.Add(_localizer["Error in saving customer."], Severity.Error);
                return;
            }

            _snackBar.Add(_localizer["Customer Saved."], Severity.Success);
            customers.Add(response.Data);
            StateHasChanged();
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
            _snackBar.Add(_localizer["Customer not found."], Severity.Error);
            return false;
        }
        protected void FilterChanged(ChangeEventArgs args)
        {
            searchString = args.Value.ToString();
            //table.ReloadServerData();
        }

        private string GetTotals(Customer customer)
        {
            var paisDebts = int.Parse(customer.CustomerDebts.PaisDebts);
            var winnerDebts = int.Parse(customer.CustomerDebts.WinnerDebts);
            var storeDebts = int.Parse(customer.CustomerDebts.StoreDebts);

            var totals = paisDebts + winnerDebts + storeDebts;
            return totals.ToString();
        }

        private string GetActionDate(Customer customer)
        {
            return DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy  HH:mm:ss");
        }
    }
}
