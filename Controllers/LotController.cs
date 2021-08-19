using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFme.Models;
using Microsoft.AspNetCore.Mvc;

namespace BFme.Controllers
{
    public class LotController : Controller
    {
        private LotContext lotContext;

        public LotController(LotContext lotContext)
        {
            this.lotContext = lotContext;
        }


        public IActionResult Index()
        {
            ViewBag.Lots = lotContext.Lots;
            return View("Index");
        }

        [HttpGet]
        public IActionResult AddLot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLot(Lot lot)
        {
            try
            {
                /*Lot lot = new Lot();
                lot.Name = Request.Form["name"];
                lot.StartCost = float.Parse(Request.Form["startCost"]);
                lot.ProjectedCost = float.Parse(Request.Form["projectedCost"]);
                lot.MarketCost = float.Parse(Request.Form["marketCost"]);
                lot.AuctionDate = DateTime.Parse(Request.Form["auctionDate"]);
                lot.DocumentSetType = Request.Form["documentSetType"];
                lot.Description = Request.Form["description"];
                lot.Assessment = Request.Form["assessment"];
                lot.Assignment = Request.Form["assignment"];
                lot.Neighbors = Request.Form["neighbors"];
                lot.ProjectedSale = Request.Form["projectedSale"];*/

                lotContext.Lots.Add(lot);
                await lotContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [NonAction]
        string ValidateString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input;
        }
        float ValidateFloat(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return 0;
                return float.Parse(input);
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
    }
}