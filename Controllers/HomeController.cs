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

        public IActionResult Index(int page = 1, string message = "")
        {
            ViewBag.Message = message;

            if (page < 1) page = 1;

            int minRow = rowsPerPage * (page - 1);
            int maxRow = rowsPerPage * (page);

            ViewBag.CurrentPage = page;
            ViewBag.Lots = db.Lots.Where(l => (l.Id > minRow) && (l.Id <= maxRow));

            return View("Index");
        }

        #region Lot

        [HttpGet]
        public IActionResult Lot(int Id)
        {
            Lot lot = db.Lots.SingleOrDefault(l => l.Id == Id);
            if(lot == null)
            {
                ViewBag.Message = "Не найдено лота с индексом " + Id;
            }

            ViewBag.SelectedLot = lot;
            return View("Lot");
        }

        [HttpGet]
        public IActionResult AddLot()
        {
            return View("AddLot");
        }

        [HttpPost]
        public async Task<IActionResult> AddLot(Lot lot)
        {
            try
            {
                Lot dbLot = db.Lots.SingleOrDefault(l => l.Id == lot.Id);
                if (dbLot != null)
                {
                    ViewBag.Message = "Данный лот уже существует";
                }
                else
                {
                    lot.Id = db.Lots.Count() + 1;
                    db.Lots.Add(lot);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Не удалось лобавить лот";
            }

            return Lot(lot.Id);
        }

        [HttpGet]
        public IActionResult EditLot(int Id)
        {

            Lot lot = db.Lots.SingleOrDefault(l => l.Id == Id);
            if (lot == null)
            {
                ViewBag.Message = "Не удалось найти данный лот в БД";
            }

            ViewBag.SelectedLot = lot;
            return View("EditLot");
        }

        [HttpPost]
        public async Task<IActionResult> EditLot(Lot lot)
        {
            try
            {
                db.Lots.Update(lot);
                await db.SaveChangesAsync();  
            }
            catch (Exception)
            {
                ViewBag.Message = "Не удалось отредактировать данный лот";
            }

            return Lot(lot.Id);
        }

        #endregion

        #region InvestСoncept

        [HttpGet]
        public IActionResult InvestСoncept(int Id)
        {
            InvestСoncept ic = db.InvestСoncepts.SingleOrDefault(l => l.Id == Id);
            if (ic == null)
            {
                ViewBag.Message = "Не найдено инвест идеи с индексом " + Id;
            }

            ViewBag.SelectedInvestСoncept = ic;
            return View("InvestСoncept");
        }

        [HttpGet]
        public IActionResult AddInvestСoncept()
        {
            return View("AddInvestСoncept");
        }

        [HttpPost]
        public async Task<IActionResult> AddInvestСoncept(InvestСoncept ic)
        {
            try
            {
                InvestСoncept dbic = db.InvestСoncepts.SingleOrDefault(l => l.Id == ic.Id);
                if (dbic != null)
                {
                    ViewBag.Message = "Данная инвест идея уже существует";
                }
                else
                {
                    ic.Id = db.InvestСoncepts.Count() + 1;
                    db.InvestСoncepts.Add(ic);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                
            }
            return InvestСoncept(ic.Id);
        }

        [HttpGet]
        public IActionResult EditInvestСoncept(int Id)
        {

            InvestСoncept ic = db.InvestСoncepts.SingleOrDefault(l => l.Id == Id);
            if (ic == null)
            {
                ViewBag.Message = "Не удалось найти данyю инвест идею в БД";
            }

            ViewBag.SelectedInvestСoncept = ic;
            return View("EditInvestСoncept");
        }

        [HttpPost]
        public async Task<IActionResult> EditInvestСoncept(InvestСoncept ic)
        {
            try
            {
                db.InvestСoncepts.Update(ic);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                ViewBag.SelectedInvestСoncept = "Не удалось отредактировать данную инвест идею";
            }

            return InvestСoncept(ic.Id);
        }

        #endregion 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
