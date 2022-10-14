using BusinessService.Reports;
using CafeErez.Client.Pages.Reports;
using CafeErez.Shared.Model.Identity;
using Infrastructure.Extensions;
using Microsoft.JSInterop;
using MudBlazor;
using System.Buffers.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;

namespace CafeErez.Client.Pages.Identity
{
    public partial class Users
    {
        private List<UserResponse> _userList = new();
        private UserResponse _user = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateUsers;
        private bool _canSearchUsers;
        private bool _canExportUsers;
        private bool _canViewRoles;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await GetUsersAsync();
            _loaded = true;
        }

        private async Task GetUsersAsync()
        {
            var response = await _userManager.GetAllUsersAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task ExportToPDF()
        {
            var pdfDocument = _pdfService.CreatePdf(_userList);

            try
            {
                await JSRuntime.InvokeAsync<byte[]>(
                "downloadFromByteArray",
                new
                {
                    ByteArray = pdfDocument.ToArray(),
                    FileName = "CustomersList.pdf",
                    ContentType = "application/pdf"
                });
            }
            catch (Exception ex)
            {
                _snackBar.Add(_localizer["Users exported Failed"], Severity.Error);
                return;
            }

            _snackBar.Add(_localizer["Users exported Success"], Severity.Success);
        }

        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private void ViewProfile(string userId)
        {
            _navigationManager.NavigateTo($"/user-profile/{userId}");
        }

        private void ManageRoles(string userId, string email)
        {
            if (email == "mukesh@blazorhero.com") _snackBar.Add(_localizer["Not Allowed."], Severity.Error);
            else _navigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }
    }
}
