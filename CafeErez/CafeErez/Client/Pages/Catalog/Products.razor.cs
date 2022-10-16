using CafeErez.Client.Pages.Identity;
using CafeErez.Client.Shared.Dialogs;
using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Model.Product;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;

namespace CafeErez.Client.Pages.Catalog
{
    public partial class Products
    {
        private List<Product> _productsList = new();
        private Product _product = new();
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            await GetProductsAsync();
            _loaded = true;
        }

        private async Task GetProductsAsync()
        {
            var response = await _productHandler.GetAllProducts();
            if (response.Succeeded)
            {
                _productsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeCreateOrEditRoleModal(string productId = null)
        {
            var parameters = new DialogParameters();
            if (productId != null)
            {
                _product = _productsList.FirstOrDefault(c => c.ProductId == productId);
                if (_product != null)
                {
                    parameters.Add(nameof(AddEditProductModal.AddEditProductModel), new Product
                    {
                        ProductId = _product.ProductId,
                        ProductDescription = _product.ProductDescription,
                        Price = _product.Price,
                        AdditionalIdentifier = _product.AdditionalIdentifier
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductModal>(productId == null ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Delete(string productId)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, productId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _productHandler.DeleteAsync(productId);
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

        private async Task ExportToPDF()
        {
            var pdfDocument = _pdfService.CreatePdf(_productsList);

            try
            {
                await JSRuntime.InvokeAsync<byte[]>(
                "downloadFromByteArray",
                new
                {
                    ByteArray = pdfDocument.ToArray(),
                    FileName = "Products.pdf",
                    ContentType = "application/pdf"
                });
            }
            catch (Exception ex)
            {
                _snackBar.Add(_localizer["Products exported Failed"], Severity.Error);
                return;
            }

            _snackBar.Add(_localizer["Products exported Success"], Severity.Success);
        }

        private async Task Reset()
        {
            _product = new Product();
            await GetProductsAsync();
        }

        private bool Search(Product product)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (product.ProductDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (product.ProductId?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

    }
}
