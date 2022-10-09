using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Product
{
    public class Product
    {
        public string ProductId{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string ProductDescription { get; set; }
        public string? AdditionalIdentifier { get; set; }
    }
}
