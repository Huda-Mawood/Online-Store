using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Online_Store.Models
{
    public enum Category
    {
        Clothes, Electronics, Foods, Furnitures, Books
    }
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required, StringLength(25, MinimumLength = 3, ErrorMessage = "Product Name can't be less than 3 longer than 20  characters")]
        public string ProductName { get; set; }

        [Required, StringLength(400, MinimumLength = 0, ErrorMessage = "Description can't be less than 0 longer than 20  characters")]

        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Price should contain numbers only")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "The Category field is required.")]
        public Category? Category { get; set; }

        [DefaultValue("default.jpg")]
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? clientFile { get; set; }



        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Stock should contain numbers only")]
        [Range(0, 100000)]
        public int Stock { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+$", ErrorMessage = "Brand name should contain characters and numbers only not any special characters")]
        public string Brand { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<ProductCart> ProductCarts { get; set; }
    }
}
