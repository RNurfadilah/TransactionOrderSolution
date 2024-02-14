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
        [Required]
        [StringLength(50)]
        public string TransactionID { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string CashierName { get; set; }

        public float? TotalAmount { get; set; }
    }
}
