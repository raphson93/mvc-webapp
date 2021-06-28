using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using mvc_webapp.Models;
using System;

namespace mvc_webapp.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        void connectionString()
        {
            con.ConnectionString = "Server=rotodb.c6nitrkqqrja.us-east-2.rds.amazonaws.com;Database=cls-db;User ID=admin;Password=root12345;";
        }
        public ActionResult Verify(Login acc)
        {
            try
            {
                connectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "select * from Login where Username='" + acc.Username + "' and Password='" + acc.Password + "'";
                dr = com.ExecuteReader();
                if(dr.Read())
                {
                    con.Close();
                    return View();
                }
                else 
                {
                    con.Close();
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return View();
        }

    }
}
