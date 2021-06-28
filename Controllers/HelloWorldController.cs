using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mvc_webapp.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            //return "This is my default action.";
            return View();
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            //return "This is the Welcome action method.";
            //return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}
