using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionOrderWeb.Models
{
    public class OrderDetail
    {
        public string TransactionDetailID { get; set; }
        public string TransactionID { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public float TotalPrice { get; set; }
    }
}