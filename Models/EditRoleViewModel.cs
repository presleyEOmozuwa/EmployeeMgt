using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMgt.Models
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}