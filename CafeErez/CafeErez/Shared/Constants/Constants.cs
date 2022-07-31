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

        public static class Customers
        {
            public const string GetCustomers = "api/customer/GetCustomers";
            public const string SaveCustomer = "api/customer/Save";
            public const string UpdateCustomer = "api/customer/Update";
            public const string DeleteCustomer = "api/customer/Delete";
            public static string GetCustomerById(int id)
            {
               return $"api/customer/{id}";
            }
        }

        public static class Roles
        {
            public const string BasicRole = "Basic";
            public const string AdministratorRole = "Administrator";
        }
        public static class Users
        {
            public const string DefaultPassword = "Aa123456!";
        }
    }
}
