using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNS.Models
{
    public class Accounts
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Holder")]
        public string AccountHolder { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [DataType(DataType.CreditCard)]
        [Display(Name = "Card Number")]
        public long CardNumber { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}
