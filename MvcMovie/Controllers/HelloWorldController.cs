using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        //.Get ./HelloWorld/
        /*
        public string Index()
        {
            return "This is my first project";
        }
        */
        public IActionResult Index()
        {
            return View();
        }
        // 
        // GET: /HelloWorld/Welcome/ 
        /*
        public string Welcome()
        {
            return "Welcome!! This is my first welcome controller method";

        }
        */
        // 
        // GET: /HelloWorld/Welcome1/
        public IActionResult welcome(string name , int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }

        public string welcome2(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {ID}");
        }
       
        
       
    }
}
