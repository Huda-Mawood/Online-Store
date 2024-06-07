using System.ComponentModel.DataAnnotations;
namespace Online_Store.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
