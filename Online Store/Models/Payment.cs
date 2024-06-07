using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Store.Models
{
    public enum PaymentWay
    {
        Cash, Visa
    }
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PaymentId { get; set; }

        [Required(ErrorMessage = "The Payment field is required.")]
        public PaymentWay? PaymentWay { get; set; }
        public int OrderID { get; set; }
        public virtual Order Orders { get; set; }

    }
}
