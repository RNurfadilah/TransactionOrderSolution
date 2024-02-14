using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionOrderAPI.Models
{
    [Table("Order", Schema = "dbo")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        [StringLength(50)]
        public string TransactionID { get; set; } = Guid.NewGuid().ToString();

        public DateTime TransactionDate { get; set; }

        [StringLength(50)]
        public string TransactionNumber { get; set; }

        [StringLength(50)]
        public string CashierName { get; set; }

        public float? TotalAmount { get; set; }
    }
}
