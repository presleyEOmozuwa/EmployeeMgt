using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EmployeeMgt.Models
{
    public class RoleModificationViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
        public List<UserRoleViewModel> Obj { get; set; }
    }
}