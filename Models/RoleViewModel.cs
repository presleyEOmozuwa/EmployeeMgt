using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EmployeeMgt.Models
{
    public class RoleViewModel : IdentityRole
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }    
    }
}