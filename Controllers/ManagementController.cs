using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMgt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgt.Controllers
{
    public class ManagementController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public ManagementController (RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var model = roleManager.Roles;
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View (model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Sorry, role with Id = {Id} could not be found";
                return RedirectToAction("NotFound");
            } 
            else
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("ListRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            if(role == null)
            {
                ViewBag.ErrorMessage($"Sorry, the role with Id = {Id} could not be found");
                return RedirectToAction("NotFound");
            }

            var model = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);

                if(role == null)
                {
                    ViewBag.ErrorMessage($"Sorry, the role with Id = {model.Id} could not be found");
                    return RedirectToAction("NotFound");
                }

                role.Id = model.Id;
                role.Name = model.RoleName;

                var result = await roleManager.UpdateAsync(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }    
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RoleUsersMembership(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage($"Sorry, the role with Id = {roleId} could not be found");
                return RedirectToAction("NotFound");
            }
            var model = new RoleModificationViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            model.Users = new List<string>();
            model.Obj = new List<UserRoleViewModel>();

            foreach(var user in userManager.Users.ToList())
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            foreach(var user in userManager.Users.ToList())
            {
                var userRole = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                model.Obj.Add(userRole);
            }

            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> RoleUsersMembership(RoleModificationViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            if(role == null)
            {
                ViewBag.ErroMessage = $"Sorry, the role with the Id = {model.RoleId} could not be found";
                return RedirectToAction("NotFound");
            }
            for(int i = 0; i < model.Obj.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model.Obj[i].UserId);
                
                IdentityResult result = new IdentityResult();
                
                if(model.Obj[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model.Obj[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                
                if(result.Succeeded)
                {
                    if(i < model.Obj.Count - 1)
                        continue;
                    else
                        return RedirectToAction("RoleUsersMembership", new {roleId = model.RoleId});    
                }
            }
            return RedirectToAction("RoleUsersMembership", new {roleId = model.RoleId}); 
        }
    }
}