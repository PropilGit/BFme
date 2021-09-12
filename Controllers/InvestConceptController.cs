using BFme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Controllers
{
    public class InvestConceptController : Controller
    {
        private InvestContext db;

        public InvestConceptController(InvestContext investContext)
        {
            this.db = investContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(int Id, string Message = "")
        {
            ViewBag.Message = Message;

            InvestConcept ic = db.InvestConcepts.SingleOrDefault(i => i.Id == Id);
            if (ic == null)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не найдено инвест идеи с индексом " + Id });
            }
            ic.Expenses = db.Expenses.Where(i => i.InvestConceptId == Id).ToList();
            ViewBag.SelectedInvestConcept = ic;
            return View("Index");
        }

        #region Add

        [Authorize]
        [HttpGet]
        public IActionResult Add(int LotId, string Message = "")
        {
            ViewBag.Message = Message;
            InvestConcept ic = new InvestConcept();
            ic.LotId = LotId;
            ViewBag.SelectedInvestConcept = ic;

            ViewBag.Action = "Add";
            return View("Edit");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(InvestConcept NewInvestConcept)
        {
            try
            {
                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(i => i.Id == NewInvestConcept.Id);
                if (dbic != null)
                {
                    return Index(NewInvestConcept.Id, "Попытка добавить уже существующую инвест идею");
                }

                Lot dblot = db.Lots.SingleOrDefault(l => l.Id == NewInvestConcept.LotId);
                if (dblot == null)
                {
                    return RedirectToAction("Index", "Home", new { Page = 1, Message = "Попытка добавить инвест идею в несуществующий лот" });
                }

                NewInvestConcept.Id = db.InvestConcepts.Count() + 1;
                db.InvestConcepts.Add(NewInvestConcept);
                await db.SaveChangesAsync();

                return Index(NewInvestConcept.Id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось добавить инвест идею" });
            }
        }

        #endregion

        #region Edit

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int Id, string Message = "")
        {

            InvestConcept ic = db.InvestConcepts.SingleOrDefault(l => l.Id == Id);
            if (ic == null)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось найти инвест идею в БД" });
            }
            ViewBag.SelectedInvestConcept = ic;

            ViewBag.Action = "Edit";
            return View("Edit");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(InvestConcept NewInvestConcept)
        {
            try
            {
                db.InvestConcepts.Update(NewInvestConcept);
                await db.SaveChangesAsync();

                return Index(NewInvestConcept.Id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Lot", new { Page = NewInvestConcept.LotId, Message = "Не удалось отредактировать инвест идею" });
            }
        }

        #endregion
    }
}
