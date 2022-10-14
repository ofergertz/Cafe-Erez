using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Constants
{
    public static class Constants
    {
        public static class IdentityAuthenticationApi
        {
            public const string Login = "api/identity/Login";
            public const string Register = "api/identity/Register";
        }

        public static class Local
        {
            public static string Preference = "clientPreference";

            public static string AuthToken = "authToken";
            public static string UserImageURL = "userImageURL";
        }

        public static class Products
        {
            public static string PriceQuery(string ProductId)
            {
                return $"api/products/PriceQuery/{ProductId}";
            }

            public const string AddProduct = "api/products/AddProduct";


        }
        public static class Customers
        {
            public const string GetCustomers = "api/customer/GetCustomers";
            public const string SaveCustomer = "api/customer/Save";
            public const string UpdateCustomer = "api/customer/Update";
            public const string DeleteCustomer = "api/customer/Delete";
            public static string GetCustomerById(string id)
            {
               return $"api/customer/GetById/{id}";
            }
        }

        public static class Roles
        {
            public const string BasicRole = "Basic";
            public const string AdministratorRole = "Administrator";
            public const string GetAll = "api/identity/roleClaim/GetAllRoles";
            public const string SaveRole = "api/identity/roleClaim/SaveRole";
            public const string DeleteRole = "api/identity/roleClaim/DeleteRole";
        }
        public static class Users
        {
            public const string DefaultPassword = "Aa123456!";
            public const string GetAllUsers = "api/identity/user";
        }

        public static class StorageConstants
        {
            public const string LocalPreference = "LocalPreference";
            public const string IsRtl = "IsRtl";
        }

    }
}
