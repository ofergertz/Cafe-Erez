using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CafeErez.Shared.Model.Customer
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string firstName, 
            string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "FirstName length can't be more than 8.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "LastName length can't be more than 8.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "PhoneNumber length can't be less/more than 10.", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public List<CustomerDebts> CustomerDebts { get; set; } = new();
    }
}
