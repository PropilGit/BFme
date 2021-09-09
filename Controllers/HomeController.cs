using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BFme.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using BFme.Services;
using System.IO;

namespace BFme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        int rowsPerPage = 10;
        private InvestContext db;
        private IFileController ftp;

        public HomeController(ILogger<HomeController> logger, InvestContext investContext, IFileController ftp)
        {

            _logger = logger;
            this.db = investContext;
            this.ftp = ftp;
        }

        public IActionResult Index(int Page = 1, string Message = "")
        {
            ViewBag.Message = Message;

            if (Page < 1) Page = 1;

            int minRow = rowsPerPage * (Page - 1);
            int maxRow = rowsPerPage * (Page);

            ViewBag.CurrentPage = Page;
            ViewBag.Lots = db.Lots.Where(l => (l.Id > minRow) && (l.Id <= maxRow));

            return View("Index");
        }
        public IActionResult Error(string Message = "")
        {
            ViewBag.Message = Message;
            return View();
        }

        #region InvestConcept

        
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

            Expense exp = new Expense();
            exp.InvestConceptId = InvestConceptId;
            ViewBag.SelectedExpense = exp;

            ViewBag.Action = "AddExpense";
            return View("EditExpense");
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(Expense exp)
        {
            try
            {
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

                exp.Id = db.Expenses.Count() + 1;
                db.Expenses.Add(exp);
                await db.SaveChangesAsync();

                return Expense(exp.Id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "InvestConcept", new { Id = exp.InvestConceptId, Message = "Не удалось добавить расход" });
            }
        }

        [HttpGet]
        public IActionResult EditExpense(int Id)
        {
            Expense exp = db.Expenses.SingleOrDefault(l => l.Id == Id);
            if (exp == null)
            {
                return Index(1, "Не удалось найти расход с индексом " + Id + " в БД");
            }
            else
            {
                ViewBag.SelectedExpense = exp;
                ViewBag.Action = "EditExpense";
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

        #region Files

        [HttpPost]
        public IActionResult Download(int Id)
        {
            LotFile dblf = db.Files.SingleOrDefault(i => i.Id == Id);
            if (dblf == null) return Index(1, "Файл не найден");

            byte[] bytes = ftp.Download(Id.ToString());
            return new FileContentResult(bytes, "application/txt")
            {
                FileDownloadName = dblf.Name
            };
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int LotId)
        {
            try
            {
                Lot dblot = db.Lots.SingleOrDefault(i => i.Id == LotId);
                if(dblot == null)
                {
                    return RedirectToAction("Index", "Home", new { LotId = LotId, Message = "Попытка добавить файл в несуществующий лот" });
                }

                //проверка существует ли файл с индексом

                //-------
                MemoryStream mStream = new MemoryStream();
                file.OpenReadStream().CopyTo(mStream);

                int Id = db.Files.Count() + 1;
                if (ftp.Upload(Id.ToString(), mStream.ToArray()))
                {
                    LotFile lf = new LotFile();
                    lf.Id = Id;
                    lf.Name = file.FileName;
                    lf.LotId = LotId;

                    db.Files.Add(lf);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Lot", new { LotId = LotId, Message = "Файл успешно загружен" });
                }
                else
                {
                    return RedirectToAction("Index", "Lot", new { LotId = LotId, Message = "Ошибка при загрузке файла на сервер" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Lot", new { LotId = LotId, Message = "Не удалось загрузить файл" });
            }  
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
