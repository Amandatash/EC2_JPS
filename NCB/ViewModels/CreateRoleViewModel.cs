using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NCB.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
