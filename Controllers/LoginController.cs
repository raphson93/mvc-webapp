using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc_webapp.Models;
using System;
using System.Net.NetworkInformation;

namespace mvc_webapp.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        private string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Console.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
        public LoginController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(Login user)
        {
            var _tokenProvider = new TokenProvider(_context);
            var (userToken, sessionEnd) = _tokenProvider.LoginUser(user);
            if (userToken != null)
            {
                //Save token in session object
                HttpContext.Session.SetString("JWToken", userToken);
                HttpContext.Session.SetString("Expired", sessionEnd.ToString());
            }
            else
            {
                ModelState.AddModelError("Error", "Invalid Username/Password");
                return View("Index");
            }
            return Redirect($"..{HttpContext.Session.GetString("OriginUrl")}");
        }

        [HttpPost]
        public JsonResult Logout([FromBody]AuthenticationBody authenticationBody)
        {
            var _tokenProvider = new TokenProvider(_context);
            var userToken = _tokenProvider.LogoutUser(authenticationBody.User);
            HttpContext.Session.SetString("JWToken", "");
            HttpContext.Session.Clear();
            return Json(Url.Action("Index", "Reports"));
        }
    }

    public class AuthenticationBody
    {
        public string User { get; set; }
        public string JWToken { get; set; }

    }
}
