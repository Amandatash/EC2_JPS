using System.ComponentModel.DataAnnotations;

namespace BNS.ViewModels
{
    public class ViewCustomerViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "Account Holder")]
        public string AccountHolder { get; set; }

        [DataType(DataType.CreditCard)]
        [Display(Name = "Card Number")]
        public long CardNumber { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}
