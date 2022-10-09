using CafeErez.Client.Shared.Extensions;
using CafeErez.Shared.Model.Customer;
using CafeErez.Shared.Model.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CafeErez.Client.Pages.CustomerScreen
{
    public partial class CustomerDiary
    {
        private List<Customer> customers = new List<Customer>();
        private List<Customer> existingCustomers = new List<Customer>();
        private string searchString = "";
        private Product ProductFromPriceQuery = new();
        private List<Tuple<Customer,CustomerDebts>> customerDebts = new List<Tuple<Customer, CustomerDebts>>();

        protected override async Task OnInitializedAsync()
        {
            existingCustomers = await GetCustomers();
            var cust = existingCustomers.ToList().Select(x => x).Where(x => x.CustomerDebts.Any()).ToList();
			foreach (var customer in cust)
			{
                foreach (var debt in customer.CustomerDebts.Where(x =>x.ActionDate.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString()))
                {
                    customerDebts.Add(new Tuple<Customer, CustomerDebts>(customer, debt));
                }
            }
        }

        private bool GetDebtByDate(Customer x)
        {
            foreach (var item in x.CustomerDebts)
            {
                Console.WriteLine(item.ActionDate.Date.ToShortDateString());
            }

            return x.CustomerDebts.Any(y => y.ActionDate.Date.ToShortDateString()==DateTime.Now.Date.ToShortDateString());
        }

        protected void FilterChanged(ChangeEventArgs args)
        {
            searchString = args.Value.ToString();
            //table.ReloadServerData();
        }

        private async Task OnCustomerSelected(Customer customer)
        {
            if (customer == null)
                return;

            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.AddNewCustomerForDiaryDebts.NewCustomer), customer }
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
            return customer.FirstName + " " + customer.LastName;
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

        private async Task CheckPrice()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true };

            var dialog = _dialogService.Show<Shared.Dialogs.CheckProductPriceDialog>("Price Query", options);
            var result = await dialog.Result;
            return;
        }

        private async Task AddNewProduct()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true };

            var dialog = _dialogService.Show<Shared.Dialogs.AddNewProductDialog>("Price Query", options);
            var result = await dialog.Result;
            if (result.Cancelled)
                return;

            var newProduct = result.Data as Product ;
            if (newProduct == null)
                return; 

            await _productHandler.AddNewProduct(newProduct);
        }

        private async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
                return;

            var customerDebt = customer.CustomerDebts.Last();
            customerDebt.UserId = await GetUserId();
            var customerUpdated = await _customerHandler.UpdateCustomer(customer);
            if (!CustomerAlreadyExistInList(customerUpdated.Data as Customer))
            {
                customerDebts.Add(new Tuple<Customer, CustomerDebts>(customer, customerDebt));
            }

            _snackBar.Add(_localizer["Customer Updated."], Severity.Success);
            StateHasChanged();
        }

        private bool CustomerAlreadyExistInList(Customer customer)
        {
            return customerDebts.Any(x => x.Item2.CustomerDebtsId == customer.CustomerDebts.Last()?.CustomerDebtsId);
        }

        private async Task Edit(Tuple<Customer,CustomerDebts> customer)
        {
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.EditCustomerDetails.NewCustomerDebts), customer }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.EditCustomerDetails>("Edit", parameters, options);
            var result = await dialog.Result;
            await UpdateCustomer(result.Data as Customer);
        }

        private async Task GetDebts(Tuple<Customer,CustomerDebts> tuple)
        {
            var customer = await GetCustomerById(tuple.Item1.CustomerId);
            var sum = CalculateTotalDebts(customer).ToString();
            bool? result = await _dialogService.ShowMessageBox(
         "Total Debts", $"Total debts for {customer.FirstName} {customer.LastName} is: {sum}");
        }

        private async Task<string> GetUserId()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            return user.GetUserId();
        }

        private static decimal CalculateTotalDebts(Customer customer)
        {
            var refundDebts = CalculateDebtForRefund(customer);
            var otherDebts = CalculateDebtForOthers(customer);
            return otherDebts - refundDebts;
        }

        private static decimal CalculateDebtForRefund(Customer customer)
        {
            return customer.CustomerDebts.Where(x => x.Action.Equals("Refund")).Sum(x => decimal.Parse(x.ActionAmount));
            //decimal.Parse(y.ActionAmount)).Sum());
        }

        private static decimal CalculateDebtForOthers(Customer customer)
        {
            return customer.CustomerDebts.Where(x => !x.Action.Equals("Refund")).Sum(x => decimal.Parse(x.ActionAmount));
        }

        private async Task AddCustomer(Customer customer)
        {
            var customerDebt = customer.CustomerDebts.Last();
            customerDebt.UserId = await GetUserId();
            var response = await _customerHandler.SaveCustomer(customer);
            if (response.Data == null)
            {
                _snackBar.Add(_localizer["Error in saving customer."], Severity.Error);
                return;
            }

            _snackBar.Add(_localizer["Customer Saved."], Severity.Success);
            var newCustomer = response.Data as Customer;
            existingCustomers.Add(newCustomer);
            customerDebts.Add(new Tuple<Customer, CustomerDebts>(newCustomer, customerDebt));
            //customers.Add(response.Data as Customer);
            StateHasChanged();
        }

        private async Task<List<Customer>> GetCustomers()
        {
            var customersResult = await _customerHandler.GetCustomers();
            return customersResult.Data;
        }

        private async Task<Customer> GetCustomerById(int id)
        {
            var customersResult = await _customerHandler.GetCustomerById(id);
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

        private string GetActionDate(CustomerDebts customer)
        {
            return customer.ActionDate.ToString("dd/MM/yyyy  HH:mm:ss");
        }
    }
}
