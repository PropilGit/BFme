using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BFme.Models;

namespace BFme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        int rowsPerPage = 10;
        private InvestContext db;

        public HomeController(ILogger<HomeController> logger, InvestContext investContext)
        {
            _logger = logger;
            this.db = investContext;
        }

        public IActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;

            int minRow = rowsPerPage * (page - 1);
            int maxRow = rowsPerPage * (page);

            ViewBag.CurrentPage = page;
            ViewBag.Lots = db.Lots.Where(l => (l.Id > minRow) && (l.Id <= maxRow));

            return View("Index");
        }

        [HttpGet]
        public IActionResult AddLot()
        {
            ViewBag.SelectedLot = new Lot();
            return View("EditLot");
        }

        [HttpPost]
        public async Task<IActionResult> EditLot(Lot lot)
        {
            try
            {
                db.Lots.Add(lot);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return AddLot();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
