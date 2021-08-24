using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFme.Models;
using Microsoft.AspNetCore.Mvc;

namespace BFme.Controllers
{
    /*
    public class LotController : Controller
    {

        TestList test;

        public LotController(TestList test)
        {
            this.test = test;
        }

        public IActionResult Index()
        {
            ViewBag.Lots = test.Lots;
            return View();
        }

        [HttpGet]
        public IActionResult AddLot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLotPost(Lot lot)
        {
            try
            {
                test.Lots.Add(lot);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
    */
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