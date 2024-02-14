namespace TransactionOrderWeb.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int CategoryID { get; set; }
        public int Stock { get; set; }
        public string ProductImage { get; set; }
    }
}
