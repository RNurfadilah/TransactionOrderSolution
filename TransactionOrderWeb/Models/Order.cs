using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionOrderWeb.Models
{
    public class Order
    {
        public string TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNumber { get; set; }
        public string CashierName { get; set; }
        public float? TotalAmount { get; set; }
    }
}