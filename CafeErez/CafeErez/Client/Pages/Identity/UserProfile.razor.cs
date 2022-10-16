using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CafeErez.Client.Pages.Identity
{
    public partial class UserProfile
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Description { get; set; }

        private bool _active;
        private char _firstLetterOfName;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;

        private bool _loaded;

        [Parameter] public string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = Id;
            var result = await _userManager.GetUserAsync(userId);
            if (result.Succeeded)
            {
                var user = result.Data;
                if (user != null)
                {
                    _firstName = user.FirstName;
                    _lastName = user.LastName;
                    _email = user.Email;
                    _phoneNumber = user.PhoneNumber;
                    //_active = user.IsActive; add user activation
                }
                Description = _email;
                if (_firstName.Length > 0)
                {
                    _firstLetterOfName = _firstName[0];
                }
            }

            _loaded = true;
        }

        private async Task ToggleUserStatus()
        {
        }
    }
}
