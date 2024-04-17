using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JPS.Models
{
    public class Bill
    {

        [Key]
        public int BillId { get; set; }


        [Required (ErrorMessage = "Please Enter the Bill Generation Date")]
        [DataType(DataType.Date)]
        [Display(Name ="Generation Date")]
        public DateTime GenerationDate { get; set; }


        [Required(ErrorMessage = "Please Enter the Bill Due Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }



        [Required(ErrorMessage = "Please Enter the Premises Number")]
        [Display(Name = "Premises Number")]
        public string PremisesNumber { get; set; }


        [Required(ErrorMessage = "Please Enter the Customer ID")]
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }


        [Required(ErrorMessage = "Please Enter the Customer Address")]
        public string Address { get; set; }




        [Required(ErrorMessage = "Please Enter the Customer Address")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount Due")]
        public int AmountDue { get; set; }






    }
}
