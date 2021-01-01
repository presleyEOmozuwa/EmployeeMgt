using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EmployeeMgt.Models
{
    public class AppUserModificationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public List<UserRoleViewModel> Obj { get; set; }
    }
}