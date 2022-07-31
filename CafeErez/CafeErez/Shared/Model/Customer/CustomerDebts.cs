using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Customer
{
    public class CustomerDebts
    {
        public int Id { get; set; }
        public string WinnerDebts { get; set; } = "0";
        public string PaisDebts { get; set; } = "0";
        public string StoreDebts { get; set; } = "0";
        [JsonIgnore]
        public Customer? Customer { get; set; } 
    }
}
