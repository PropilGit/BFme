using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BFme.Controllers
{
    public class LotController : Controller
    {
        int rowsPerPage = 5;
        private LotContext lotContext;

        public LotController(LotContext lotContext)
        {
            this.lotContext = lotContext;
        }
        

        public IActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;

            int minRow = rowsPerPage * (page - 1);
            int maxRow = rowsPerPage * (page);

            ViewBag.Lots = lotContext.Lots.FromSqlRaw(
                "SELECT * " +
                "FROM " + lotContext.TableName + " " +
                "WHERE Id > " + minRow + " AND " + "Id <= " + maxRow
                    ).ToList();

            return View("Index");
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
                lotContext.Lots.Add(lot);
                await lotContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
    
}