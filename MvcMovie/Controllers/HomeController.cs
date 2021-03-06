using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvcMovie.Services;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger,IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }


        public async Task<ViewResult> Index()
        {
            //UserEailOptions options = new UserEailOptions
            //{
            //    ToEmails = new List<string>() { "test@gmail.com" }

            //};

            //await _emailService.SendTestemail(options);

            return View();
        }

        //public IActionResult Index()
        //{
           
        //    return View();
        //}

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
