using System;
using System.ComponentModel.DataAnnotations;

namespace BNS.Models
{
    public class Transactions
    {
        [Key]
        [Required]
        [Display(Name = "Transaction ID")]
        public int TransactionID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int Sender { get; set; }

        [Required, Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
