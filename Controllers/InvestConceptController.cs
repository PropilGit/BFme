using BFme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BFme.Controllers
{
    public class InvestConceptController : Controller
    {
        private InvestContext db;

        public InvestConceptController(InvestContext investContext)
        {
            this.db = investContext;
        }


        [HttpPost]
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Add(InvestConcept InvestConcept)
        {
            try
            {
                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(i => i.Id == InvestConcept.Id);
                if (dbic != null)
                {
                    //return Index(InvestConcept.Id, "");
                    return RedirectToAction("Index", "Lot", new { Id = dbic.LotId, Message = "Попытка добавить уже существующую инвест идею" });
                }

                Lot dblot = db.Lots.SingleOrDefault(l => l.Id == InvestConcept.LotId);
                if (dblot == null)
                {
                    return RedirectToAction("Index", "Home", new { Page = 1, Message = "Попытка добавить инвест идею в несуществующий лот" });
                }

                InvestConcept.Id = db.InvestConcepts.Max(e => e.Id) + 1; ;
                db.InvestConcepts.Add(InvestConcept);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Lot", new { Id = InvestConcept.LotId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось добавить инвест идею" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "agent")]
        public async Task<IActionResult> Edit(InvestConcept InvestConcept)
        {
            try
            {
                db.InvestConcepts.Update(InvestConcept);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Lot", new { Id = InvestConcept.LotId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Lot", new { Id = InvestConcept.LotId, Message = "Не удалось отредактировать инвест идею" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddExpense(Expense Expense)//int InvestConceptId, string Name, float SinglePayment, float MonthlyPayment
        {
            try
            {
                /*
                Expense Expense = new Expense();
                Expense.InvestConceptId = InvestConceptId;
                Expense.Name = Name;
                Expense.SinglePayment = SinglePayment;
                Expense.MonthlyPayment = MonthlyPayment;
                /*
                InvestConcept dbexp = db.InvestConcepts.SingleOrDefault(i => i.Id == exp.Id);
                if (dbexp != null)
                {
                    ViewBag.Message = "Попытка добавить уже существующую инвест идею";
                    ViewBag.Action = "EditExpense";
                    return EditExpense(exp.Id);
                }

                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(l => l.Id == exp.InvestConceptId);
                if (dbic == null)
                {
                    return Index(1, "Попытка добавить расход в несуществующую инвест идею");
                }
                */
                Expense.Id = db.Expenses.Max(e => e.Id) + 1;
                db.Expenses.Add(Expense);
                await db.SaveChangesAsync();

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }

            //return Json(result);
        }

        [HttpPost]
        //[Authorize(Roles = "agent")]
        public JsonResult GetExpenses(int Id)
        {
            List<Expense> exps = db.Expenses.Where(e => e.InvestConceptId == Id).ToList();
            return Json(exps);
        }

        [HttpPost]
        public async Task<JsonResult> Remove(int Id)//int InvestConceptId, string Name, float SinglePayment, float MonthlyPayment
        {
            try
            {
                InvestConcept dbic = db.InvestConcepts.SingleOrDefault(i => i.Id == Id);
                if (dbic == null)
                {
                    //"Попытка удалить несуществующую инвест идею";
                    return Json(false);
                }

                db.InvestConcepts.Remove(dbic);
                await db.SaveChangesAsync();

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        #region kal


        public HtmlString GetExpensesHtml(int Id)
        {
            List<Expense> exps = db.Expenses.Where(e => e.InvestConceptId == Id).ToList();

            string result = "<table><tr><<td>Категория расхода</td><td>Разово</td><td>Ежемесячно</td></tr>";
            foreach (var exp in exps)
            {
                result = "{result}<tr><td>{exp.Name}</td><td>{exp.SinglePayment}</td><td>{exp.MonthlyPayment}</td></tr></table>";
            }
            return new HtmlString(result);
        }

        #endregion
    }
}

/*
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

                NewInvestConcept.Id = db.InvestConcepts..Max(e => e.Id) + 1;;
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
        */