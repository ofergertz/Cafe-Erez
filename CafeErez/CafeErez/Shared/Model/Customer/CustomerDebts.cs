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
        public CustomerDebts()
        {
        }

        public CustomerDebts(CustomerDebts customerDebts)
        {
            CustomerDebtsId = customerDebts.CustomerDebtsId;
            Action = customerDebts.Action;
            ActionAmount = customerDebts.ActionAmount;
            Comments = customerDebts.Comments;
            ActionDate = customerDebts.ActionDate;
        }

        public int CustomerDebtsId { get; set; }
        public string Action { get; set; } = "";
        public string ActionAmount { get; set; } = "";
        public string Comments { get; set; } = "";
        public DateTime ActionDate { get; set; } = default(DateTime);
        public string UserId { get; set; } = "";

    }
}
