using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Customer
{
    public class CustomerDiary
    {
        public int CustomerDiaryId { get; set; }
        public Customer Customer { get; set; }
        public string Action { get; set; }
        public string ActionAmount { get; set; }
        public string Comments { get; set; } = "";
        public DateTime? ActionDate { get; set; }
        List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
