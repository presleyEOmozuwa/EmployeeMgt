using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMgt.Models;
using EmployeeMgt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeMgt.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMessageService messageService;
        

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMessageService messageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.messageService = messageService;
            
        }
        
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var emp = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(emp, model.Password);
                if(result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(emp);
                    var Link = Url.Action(nameof(ConfirmationEmail), "Account", new { userId = emp.Id, token}, HttpContext.Request.Scheme, Request.Host.ToString());

                    await messageService.SendEmailAsync("5e95a5373406dd", "presleyomozuwa-852e73@inbox.mailtrap.io", model.FirstName, model.Email, "Verify Email", $"Please confirm your email : <a href='{Link}'>Confirm</a>");
                    
                    return RedirectToAction("EmailSentPage");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        } 

        [HttpGet]
        public IActionResult EmailSentPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmationEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            
            if(user == null)
                return BadRequest();
            
            var result = await userManager.ConfirmEmailAsync(user, token);
            
            if(result.Succeeded)
            {
                return View();
            }

            return View();
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var model = userManager.Users;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"Sorry, user with Id = {Id} could not be found";
                return RedirectToAction("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("ListUsers");
        }
    }
}