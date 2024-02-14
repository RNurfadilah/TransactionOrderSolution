using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionOrderAPI.Models
{
    [Table("Product", Schema = "dbo")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        [StringLength(500)]
        public string ProductName { get; set; }

        public float Price { get; set; }

        public int CategoryID { get; set; }

        public int Stock { get; set; }

        [StringLength(50)]
        public string ProductImage { get; set; }

        public virtual Category Category { get; set; }
    }
}