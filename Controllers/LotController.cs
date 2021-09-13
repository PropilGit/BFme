using BFme.Models;
using BFme.Services;
using BFme.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Controllers
{
    public class LotController : Controller
    {

        private InvestContext db;
        private IFileController fc;

        public LotController(InvestContext investContext, IFileController fc)
        {
            this.db = investContext;
            this.fc = fc;
        }

        #region Index

        [HttpGet][Authorize]
        public IActionResult Index(int Id, string Message = "")
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            user.Role = db.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (user.Role.Name == "agent")
            {
                return Agent(Id, Message);
            }
            if (user.Role.Name == "rieltor")
            {
                return Rieltor(Id, Message);
            }
            else return RedirectToAction("Index", "Home", new { Message = "Ошибка Авторизации #0001" });
        }

        
        [HttpGet][Authorize(Roles = "agent")]
        public IActionResult Agent(int Id, string Message = "")
        {
            Lot lot = db.Lots.FirstOrDefault(l => l.Id == Id);
            if (lot == null) return Add("Не найдено лота с индексом " + Id);

            lot.Files = db.Files.Where(c => c.LotId == Id).ToList();
            lot.InvestConcepts = db.InvestConcepts.Where(c => c.LotId == Id).ToList();
            foreach (var ic in lot.InvestConcepts) ic.Expenses = db.Expenses.Where(c => c.InvestConceptId == ic.Id).ToList();

            return View("Index", new AgentViewModel(lot, ActionsEnum.Edit, Message));
        }

        [HttpGet][Authorize(Roles = "rieltor")]
        public IActionResult Rieltor(int Id, string Message = "")
        {
            Lot lot = db.Lots.FirstOrDefault(l => l.Id == Id);
            if (lot == null) return Add("Не найдено лота с индексом " + Id);

            return View("Review", new RieltorViewModel(lot, Message));
        }

        #endregion

        #region Add

        [HttpGet][Authorize(Roles = "agent")]
        public IActionResult Add(string Message = "")
        {
            AgentViewModel avm = new AgentViewModel(
                new Lot(),
                ActionsEnum.Add,
                Message);

            return View("Index", avm);
        }

        [HttpPost][Authorize(Roles = "agent")]
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

                    return Index(lot.Id, "Новый лот успешно создан");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось добавить лот" });
            }
        }

        #endregion

        #region Edit

        [HttpPost][Authorize(Roles = "agent")]
        public async Task<IActionResult> Edit(Lot Lot)
        {
            try
            {
                db.Lots.Update(Lot);
                await db.SaveChangesAsync();

                return Index(Lot.Id, "Изменения сохранены");
            }
            catch (Exception)
            {
                return Index(Lot.Id, "Не удалось отредактировать данный лот");
            }
        }

        [HttpPost][Authorize(Roles = "rieltor")]
        public async Task<IActionResult> EditReview(Lot Lot)
        {
            try
            {
                Lot dblot = db.Lots.Where(l => l.Id == Lot.Id).FirstOrDefault();
                if (dblot == null)
                {
                    return RedirectToAction("Index", "Home", new { Page = 1, Message = "Попытка отредактировать несуществующий лот" });
                }

                dblot.MarketCost = Lot.MarketCost;
                dblot.Review = Lot.Review;

                db.Lots.Update(dblot);
                await db.SaveChangesAsync();

                return Index(Lot.Id, "Изменения сохранены");
            }
            catch (Exception)
            {
                return Index(Lot.Id, "Не удалось отредактировать данный лот");
            }
        }

        #endregion

        #region Files

        [Authorize]
        [HttpGet]
        public IActionResult Download(int Id)
        {
            LotFile dblf = db.Files.SingleOrDefault(i => i.Id == Id);
            if (dblf == null) return Index(1, "Файл не найден");

            byte[] bytes = fc.Download(Id.ToString());
            return new FileContentResult(bytes, "application/txt")
            {
                FileDownloadName = dblf.Name
            };
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile File, int LotId)
        {
            try
            {
                Lot dblot = db.Lots.SingleOrDefault(i => i.Id == LotId);
                if (dblot == null)
                {
                    return RedirectToAction("Index", "Home", new { LotId = LotId, Message = "Попытка добавить файл в несуществующий лот" });
                }

                //проверка существует ли файл с индексом

                //-------
                MemoryStream mStream = new MemoryStream();
                File.OpenReadStream().CopyTo(mStream);

                int Id = db.Files.Count() + 1;
                if (fc.Upload(Id.ToString(), mStream.ToArray()))
                {
                    LotFile lf = new LotFile();
                    lf.Id = Id;
                    lf.Name = File.FileName;
                    lf.LotId = LotId;

                    db.Files.Add(lf);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Lot", new { Id = LotId, Message = "Файл успешно загружен" });
                }
                else
                {
                    return RedirectToAction("Index", "Lot", new { Id = LotId, Message = "Ошибка при загрузке файла на сервер" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Lot", new { Id = LotId, Message = "Не удалось загрузить файл" });
            }
        }

        #endregion
    }
}


/*
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int Id, string Message = "")
        {
            Lot lot = db.Lots.SingleOrDefault(l => l.Id == Id);
            if (lot == null)
            {
                return RedirectToAction("Index", "Home", new { Page = 1, Message = "Не удалось найти данный лот в БД" });
            }

            AgentViewModel avm = new AgentViewModel(
                lot,
                ActionsEnum.Edit,
                Message);

            return View("Index", avm);
        }
*/