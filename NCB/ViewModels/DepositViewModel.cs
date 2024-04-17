using System;
using System.ComponentModel.DataAnnotations;

namespace NCB.ViewModels
{
    public class DepositViewModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
