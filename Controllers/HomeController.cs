using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BFme.Models;
using Microsoft.EntityFrameworkCore;

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
            if (lot == null)
            {
                 return AddLot("Не найдено лота с индексом " + Id);
            }
            else
            {
                lot.InvestConcepts = db.InvestConcepts.Where(c => c.LotId == Id).ToList();
            }

            ViewBag.SelectedLot = lot;
            return View("Lot");
        }

        [HttpGet]
        public IActionResult AddLot(string message = "")
        {
            ViewBag.Message = message;
            ViewBag.SelectedLot = new Lot();
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

        #region InvestConcept

        [HttpGet]
        public IActionResult InvestConcept(int Id, string message = "")
        {
            ViewBag.Message = message;

            InvestConcept ic = db.InvestConcepts.SingleOrDefault(l => l.Id == Id);
            if (ic == null)
            {
                return Index(1, "Не найдено инвест идеи с индексом " + Id);
            }

            ViewBag.SelectedInvestConcept = ic;
            return View("InvestConcept");
        }

        [HttpGet]
        public IActionResult AddInvestConcept(int LotId, string message = "")
        {
            ViewBag.Message = message;
            ViewBag.SelectedInvestConcept = new InvestConcept();
            ViewBag.LotId = LotId;
            return View("AddInvestConcept");
        }

        [HttpPost]
        public async Task<IActionResult> AddInvestConcept(InvestConcept ic, int LotId)
        {
            try
            {
                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(i => i.Id == ic.Id);
                if (dbic != null)
                {
                    ViewBag.Message = "Попытка добавить уже существующую инвест идею";
                    return EditInvestConcept(ic.Id);
                }

                Lot dblot = db.Lots.SingleOrDefault(l => l.Id == LotId);
                if (dblot == null)
                {
                    return Index(1, "Попытка добавить инвест идею в несуществующий лот");
                }

                ic.Id = db.InvestConcepts.Count() + 1;
                db.InvestConcepts.Add(ic);
                //await db.SaveChangesAsync();

                //dblot.InvestConcepts.Add(ic);
                //db.Lots.Update(dblot);
                await db.SaveChangesAsync();

                return InvestConcept(ic.Id);
            }
            catch (Exception ex)
            {
                return Index(1, "Непредвиденная ошибка");
            }
        }

        [HttpGet]
        public IActionResult EditInvestConcept(int Id)
        {

            InvestConcept ic = db.InvestConcepts.SingleOrDefault(l => l.Id == Id);
            if (ic == null)
            {
                ViewBag.Message = "Не удалось найти данyю инвест идею в БД";
            }

            ViewBag.SelectedInvestConcept = ic;
            return View("EditInvestConcept");
        }

        [HttpPost]
        public async Task<IActionResult> EditInvestConcept(InvestConcept ic)
        {
            try
            {
                db.InvestConcepts.Update(ic);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.SelectedInvestConcept = "Не удалось отредактировать данную инвест идею";
            }

            return InvestConcept(ic.Id);
        }

        #endregion

        #region Expense

        [HttpGet]
        public IActionResult Expense(int Id)
        {
            Expense exp = db.Expenses.SingleOrDefault(l => l.Id == Id);
            if (exp == null)
            {
                return Index(1, "Не найдено расхода с индексом " + Id);
            }
            else
            {
                ViewBag.SelectedExpense = exp;
                return View("Expense");
            }
        }

        [HttpGet]
        public IActionResult AddExpense(int InvestConceptId, string message = "")
        {
            ViewBag.Message = message;
            ViewBag.SelectedExpense = new Expense();
            ViewBag.InvestConceptId = InvestConceptId;

            ViewBag.IsAdding = true;
            return View("EditExpense");
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(Expense exp, int InvestConceptId)
        {
            try
            {
                InvestConcept dbexp = db.InvestConcepts.SingleOrDefault(i => i.Id == exp.Id);
                if (dbexp != null)
                {
                    ViewBag.Message = "Попытка добавить уже существующую инвест идею";
                    return EditExpense(exp.Id);
                }

                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(l => l.Id == InvestConceptId);
                if (dbic == null)
                {
                    return Index(1, "Попытка добавить расход в несуществующую инвест идею");
                }

                exp.Id = db.Expenses.Count() + 1;
                db.Expenses.Add(exp);
                await db.SaveChangesAsync();

                return Expense(exp.Id);
            }
            catch (Exception ex)
            {
                return InvestConcept(InvestConceptId, "Не удалось добавить расход");
            }
        }

        [HttpGet]
        public IActionResult EditExpense(int Id)
        {

            Expense exp = db.Expenses.SingleOrDefault(l => l.Id == Id);
            if (exp == null)
            {
                return Index(1, "Не удалось найти расход с индексом "+Id+" в БД");
            }
            else
            {
                ViewBag.SelectedExpense = exp;
                return View("EditExpense");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditExpense(Expense exp)
        {
            try
            {
                db.Expenses.Update(exp);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.SelectedExpense = "Не удалось отредактировать данный расход";
            }

            return Expense(exp.Id);

        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
