using System;
using System.ComponentModel.DataAnnotations;

namespace BNS.ViewModels
{
    public class DepositViewModel
    {
        [Required]
        public int Account { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
