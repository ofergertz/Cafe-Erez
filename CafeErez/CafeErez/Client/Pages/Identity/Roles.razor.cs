using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Model.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.Security.Claims;

namespace CafeErez.Client.Pages.Identity
{
    public partial class Roles
    {
        [Inject] private IRoleManager RoleManager { get; set; }

        private List<RoleResponse> _roleList = new();
        private RoleResponse _role = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            await GetRolesAsync();
            _loaded = true;
        }

        private async Task GetRolesAsync()
        {
            var response = await RoleManager.GetRolesAsync();
            if (response.Succeeded)
            {
                _roleList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(string id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await RoleManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task InvokeCreateOrEditRoleModal(string id = null)
        {
            var parameters = new DialogParameters();
            if (id != null)
            {
                _role = _roleList.FirstOrDefault(c => c.Id == id);
                if (_role != null)
                {
                    parameters.Add(nameof(RoleModal.RoleModel), new RoleRequest
                    {
                        Id = id,
                        Name = _role.RoleName,
                        Description = _role.Description
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<RoleModal>(id == null ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _role = new RoleResponse();
            await GetRolesAsync();
        }

        private bool Search(RoleResponse role)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (role.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (role.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private void ManagePermissions(string roleId)
        {
            _navigationManager.NavigateTo($"/identity/role-permissions/{roleId}");
        }
    }
}
