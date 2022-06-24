using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_webapp.Models;
using mvc_webapp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace mvc_webapp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly DataContext _context;

        public ReportsController(DataContext context)
        {
            _context = context;
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var claim = HttpContext.User.Claims;
            var sessionToken = HttpContext.Session.GetString("JWToken");
            DateTime.TryParse(HttpContext.Session.GetString("Expired"), out var sessionEnd);
            if (string.IsNullOrWhiteSpace(sessionToken) || sessionEnd < DateTime.Now)
            {
                HttpContext.Session.SetString("OriginUrl", Request.Path);
                return Redirect("~/Login/Index");
            }

            return View();
        }

        /*[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Privacy()
        {
            return View();
        }*/

        public IActionResult Privacy()
        {
            var claim = HttpContext.User.Claims;
            var sessionToken = HttpContext.Session.GetString("JWToken");
            DateTime.TryParse(HttpContext.Session.GetString("Expired"), out var sessionEnd);
            if (string.IsNullOrWhiteSpace(sessionToken) || sessionEnd < DateTime.Now)
            {
                HttpContext.Session.SetString("OriginUrl", Request.Path);
                return Redirect("~/Login/Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Reports
        public JsonResult GetData(JqueryDatatableParam param)
        {
            var query = _context.Report.Select(x => new ReportViewModel(x)).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(param.sSearch))
            {
                //query from table
                query = query.Where(x => x.MachId.Contains(param.sSearch, StringComparison.OrdinalIgnoreCase)
                || x.Description.Contains(param.sSearch, StringComparison.OrdinalIgnoreCase)
                || x.FlmSlm.Contains(param.sSearch, StringComparison.OrdinalIgnoreCase)
                || x.ProblemCode.Contains(param.sSearch, StringComparison.OrdinalIgnoreCase)
                || x.Date.ToString().Contains(param.sSearch, StringComparison.OrdinalIgnoreCase));

            }

            var sortColumnIndex = param.iSortCol_0;
            var sortDirection = param.sSortDir_0;

            query = sortColumnIndex switch
            {
                0 => sortDirection == "asc" ? query.OrderBy(x => x.MachId) : query.OrderByDescending(x => x.MachId),
                1 => sortDirection == "asc" ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date),
                2 => sortDirection == "asc" ? query.OrderBy(x => x.ArrivalTime) : query.OrderByDescending(x => x.ArrivalTime),
                3 => sortDirection == "asc" ? query.OrderBy(x => x.ProblemCode) : query.OrderByDescending(x => x.ProblemCode),
                4 => sortDirection == "asc" ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description),
                5 => sortDirection == "asc" ? query.OrderBy(x => x.FlmSlm) : query.OrderByDescending(x => x.FlmSlm),
                _ => query.OrderBy(x => x)
            };

            var displayResult = query
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength)
                .ToList();

            var totalRecords = query.Count();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,REF_NO,FLM_SLM,MACH_ID,SERIAL_NO,LOCATION,DATE,ARRIVAL_TIME,PROBLEM_CODE,DESCRIPTION")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,REF_NO,FLM_SLM,MACH_ID,SERIAL_NO,LOCATION,DATE,ARRIVAL_TIME,PROBLEM_CODE,DESCRIPTION")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.FindAsync(id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }


    }
}
