using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Online_Store.Models
{
    public class ProductCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductCartId { get; set; }

        public int CartId { get; set; }
        public int ProductId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Quantity should contain numbers only")]
        public int Quantity { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
