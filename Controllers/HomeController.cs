using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_webapp.Models;
using System.Data.OleDb;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace mvc_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            var sessionToken = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                HttpContext.Session.SetString("OriginUrl", Request.Path);
                return Redirect("~/Login/Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ImportFile(IFormFile importFile)
        {
            if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });

            try
            {
                var fileData = GetDataFromCSVFile(importFile.OpenReadStream());

                var dtEmployee = fileData;
                var tblEmployeeParameter = new SqlParameter("tblEmployeeTableType", SqlDbType.Structured)
                {
                    TypeName = "dbo.DN",
                    Value = dtEmployee
                };
                await _context.Database.ExecuteSqlRawAsync("EXEC spBulkImportEmployee @tblDN", tblEmployeeParameter);
                return Json(new { Status = 1, Message = "File Imported Successfully " });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }

        private List<DN> GetDataFromCSVFile(Stream stream)
        {
            var dnList = new List<DN>();
            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // To set First Row As Column Names    
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                            dnList.Add(new DN()
                            {
                                Request = objDataRow["Request"].ToString(),
                                ReportedDt = Convert.ToDateTime(objDataRow["Reported Date"]),
                                Tag = objDataRow["Tag"].ToString(),
                                SerialNo = objDataRow["Serial Number"].ToString(),
                                ReqType = objDataRow["Request Type"].ToString(),
                                ProbSumamry = objDataRow["Problem Summary"].ToString(),
                                WorkStart = Convert.ToDateTime(objDataRow["Work Start (DD/MM/YY HH:MM)"]),
                                WorkEnd = Convert.ToDateTime(objDataRow["Work End (DD/MM/YY HH:MM)"]),
                                Resolution = objDataRow["Resolution"].ToString(),
                                ServiceCoder = objDataRow["Service Coder"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dnList;
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
