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
        private List<Tuple<Customer,CustomerDebts>> customerDebts = new List<Tuple<Customer, CustomerDebts>>();

        protected override async Task OnInitializedAsync()
        {
            existingCustomers = await GetCustomers();
            var cust = existingCustomers.ToList().Select(x => x).Where(x => x.CustomerDebts.Any()).ToList();
            customers = cust.Where(x => x.CustomerDebts.Any(y => DateTime.Compare(y.ActionDate.Date, DateTime.UtcNow.ToLocalTime().Date) == 0)).ToList();
			foreach (var customer in customers)
			{
                foreach (var debt in customer.CustomerDebts)
                {
                    customerDebts.Add(new Tuple<Customer, CustomerDebts>(customer, debt));
                }
            }
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

        private async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
                return;

            var customerUpdated = await _customerHandler.UpdateCustomer(customer);
            if (!CustomerAlreadyExistInList(customerUpdated.Data as Customer))
                customerDebts.Add(new Tuple<Customer, CustomerDebts>(customer, customer.CustomerDebts.Last()));

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
            var sum = customer.CustomerDebts.Sum(x => int.Parse(x.ActionAmount)).ToString();
            bool? result = await _dialogService.ShowMessageBox(
         "Total Debts", $"Total debts for {customer.FirstName} {customer.LastName} is: {sum}");
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
            var newCustomer = response.Data as Customer;
            existingCustomers.Add(newCustomer);
            customerDebts.Add(new Tuple<Customer, CustomerDebts>(newCustomer,
                newCustomer.CustomerDebts.Last()));
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
        protected void FilterChanged(ChangeEventArgs args)
        {
            searchString = args.Value.ToString();
            //table.ReloadServerData();
        }

        //private string GetTotals(Customer customer)
        //{
        //    var paisDebts = int.Parse(customer.CustomerDebts.PaisDebts);
        //    var winnerDebts = int.Parse(customer.CustomerDebts.WinnerDebts);
        //    var storeDebts = int.Parse(customer.CustomerDebts.StoreDebts);

        //    var totals = paisDebts + winnerDebts + storeDebts;
        //    return totals.ToString();
        //}

        private string GetActionDate(CustomerDebts customer)
        {
            //customer.CustomerDebts.Last().ActionDate = customer.CustomerDebts?.Last()?.ActionDate ?? DateTime.UtcNow.ToLocalTime().Date;

            //return customer.CustomerDebts.Last().ActionDate.ToString("dd/MM/yyyy  HH:mm:ss");
            return customer.ActionDate.ToString("dd/MM/yyyy  HH:mm:ss");
        }
    }
}
