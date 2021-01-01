using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgt.Models
{
    public class HumanResourceContext : IdentityDbContext<AppUser>
    {
        public HumanResourceContext(DbContextOptions<HumanResourceContext> options) :base(options){}
    }
}