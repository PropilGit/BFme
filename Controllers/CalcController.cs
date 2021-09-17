using BFme.Models;
using BFme.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Controllers
{
    public class CalcController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Result(CalcController calc)
        {
            return View("Result", calc);
        }

        public IActionResult Test()
        {

            //EXP
            List<Expense> expenses = new List<Expense>() { new Expense(), new Expense(), new Expense(), new Expense(), new Expense(), new Expense() };

            string[] names = new string[6]
            {
                "Охрана",
                "Вознаграждение риелтору",
                "Коммунальные услуги",
                "Реклама",
                "Прочее",
                "Налоги"
            };
            Random rnd = new Random();
            for (int i = 0; i < expenses.Count; i++)
            {
                expenses[i].Id = i + 1;
                expenses[i].Name = names[i];
                expenses[i].SinglePayment = rnd.Next(10, 50) * 1000;
                expenses[i].MonthlyPayment = rnd.Next(5, 30) * 1000;
            }

            //IC
            List<InvestConcept> ics = new List<InvestConcept>() { new InvestConcept(), new InvestConcept(), new InvestConcept() };

            for (int i = 0; i < ics.Count; i++)
            {
                ics[i].Id = i + 1;
                ics[i].LotId = 1;
                ics[i].ImplementationPeriod = 1 + i * 3;
                ics[i].PurchasePrice = 7000000;
                ics[i].SalePrice = 8000000;
                ics[i].Expenses = expenses;
            }

            //LOT
            Lot lot = new Lot();
            lot.Id = 1;
            lot.Name = "Моска, ул.Ключевая, д18";
            lot.Description = "Нежилое помещение, назначение: нежилое, общей площадью 95,6 м2, эт. 1, помещение VII," +
                "расположенное по адресу: г.Москва, ул.Ключевая, д. 18; кадастровый номер: 77:05:0012003:24075";
            lot.LinkEFRSB = "https://bankrot.fedresurs.ru/MessageWindow.aspx?ID=434EF293A0EE84F86824CA1F2F111476";
            lot.LinkTradingPlatform = "http://www.bankrupt.centerr.ru/public/auctions/lots/view/1085301/";
            lot.AuctionPrice = 7000000;

            lot.MarketCost = 8000000;
            lot.Review = "Локация\n" +
                "Рядом с метро, около 4 мин пешком, есть парковка и рядом тоже. Спальный квартал, проходное место, как утром так и вечером.\n" +
                "Что в округе, район\n" +
                "Практически все есть.\n" +
                "Какая планировка?\n" +
                "Офисная планировка, можно разбить на 2 части, т.к.есть два выхода, клиника(хотя рядом есть инвитро), тур фирма, да и много чего социального, даже магазин можно.\n" +
                "Итого\n" +
                "Место проходное, аренда там от 2000 руб / мес, если точка раскручена, цена продажи уходит в 200 - 240 т.р. за кв.м.!";
            lot.InvestConcepts = ics;

            CalcViewModel calc = new CalcViewModel(lot);
            return View("Result", calc);
        }
    }
}
