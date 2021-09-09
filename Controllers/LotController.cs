using BFme.Models;
using BFme.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Controllers
{
    public class LotController : Controller
    {

        private InvestContext db;

        public LotController(InvestContext investContext)
        {
            this.db = investContext;
        }

        [HttpGet]
        public IActionResult Index(int Id, string message = "")
        {
            ViewBag.Message = message;
            Lot lot = db.Lots.SingleOrDefault(l => l.Id == Id);
            if (lot == null)
            {
                return Add("Не найдено лота с индексом " + Id);
            }
 
            lot.Files = db.Files.Where(c => c.LotId == Id).ToList();
            lot.InvestConcepts = db.InvestConcepts.Where(c => c.LotId == Id).ToList();
            foreach (var ic in lot.InvestConcepts) ic.Expenses = db.Expenses.Where(c => c.InvestConceptId == ic.Id).ToList();

            ViewBag.SelectedLot = lot;
            ViewBag.Action = "Edit";
            return View("Index");
        }

        #region Add

        [HttpGet]
        public IActionResult Add(string message = "")
        {
            ViewBag.Message = message;
            ViewBag.SelectedLot = new Lot();
            ViewBag.Action = "Add";
            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Lot lot)
        {
            try
            {
                Lot dbLot = db.Lots.SingleOrDefault(l => l.Id == lot.Id);
                if (dbLot != null)
                {
                    return Index(lot.Id, "Данный лот уже существует");
                }
                else
                {
                    lot.Id = db.Lots.Count() + 1;
                    db.Lots.Add(lot);
                    await db.SaveChangesAsync();

                    return Index(lot.Id);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось добавить лот" });
            }
        }

        #endregion

        #region Edit

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Lot lot = db.Lots.SingleOrDefault(l => l.Id == Id);
            if (lot == null)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось найти данный лот в БД" });
            }

            ViewBag.SelectedLot = lot;
            ViewBag.Action = "Edit";
            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lot lot)
        {
            try
            {
                db.Lots.Update(lot);
                await db.SaveChangesAsync();

                return Index(lot.Id);
            }
            catch (Exception)
            {
                return Index(lot.Id, "Не удалось отредактировать данный лот");
            }
        }

        #endregion
    }
}
