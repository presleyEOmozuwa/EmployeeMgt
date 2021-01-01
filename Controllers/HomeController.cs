using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeMgt.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeMgt.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.model= "Welcome Home";
            return View();
        }
        
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        
        
        
        
        
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
