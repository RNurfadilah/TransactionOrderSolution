using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionOrderAPI.Models
{
    [Table("OrderDetail", Schema = "dbo")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        [StringLength(50)]
        public string TransactionDetailID { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        public string TransactionID { get; set; }

        public int ProductID { get; set; }

        public int CategoryID { get; set; }

        public float TotalPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}
