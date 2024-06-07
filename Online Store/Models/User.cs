using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Online_Store.Models
{

    public enum Gender
    {
        Female, Male
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID Not Valid")]
        public String NationalID { get; set; } = "";

        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "First Name can't be less than 3 and longer than 20  characters")]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string FirstName { get; set; }

        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name can't be less than 3 and longer than 20 characters")]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string LastName { get; set; }

        [Required, StringLength(11, ErrorMessage = "Phone Number Must be 11 Digit")]
        public string Phone { get; set; } = "";

        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "User Name can't be less than 3 and longer than 20 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+$")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        //[StringLength(16, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        ////[StringLength(15, ErrorMessage = "Must be between 7 and 15 characters", MinimumLength = 7)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(64)] // Assuming SHA256 hash will be 64 bytes long
        public byte[] Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [NotMapped] // Not mapped to database
        [Compare("Password", ErrorMessage = "Password not correct")]
        public byte[] ConfirmPassword { get; set; }


        [StringLength(100, MinimumLength = 4, ErrorMessage = "Add True Address can't be less than 4 longer than 100 characters")]
        public string Address { get; set; } = "";
        [Required]
        public Gender Gender { get; set; }

        [CreditCard]
        public string CreditCardNumber { get; set; } = "";



        public ICollection<Order> Orders { get; set; }
        public virtual Cart Carts { get; set; }


    }
}
