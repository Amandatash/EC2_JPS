using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NCB.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        [Display(Name ="Role ID")]
        public string RoleId { get; set; }

        [Display(Name ="Role Name")]
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
