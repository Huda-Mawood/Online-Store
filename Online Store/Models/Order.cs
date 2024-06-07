using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
namespace Online_Store.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        // [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Required]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Price should contain numbers only")]
        public decimal TotalPrice { get; set; }


        public int UserID { get; set; }
        public virtual Payment Payments { get; set; }
        public virtual User Users { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
