using BFme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.ViewModels
{
    public class CalcViewModel
    {

        public CalcViewModel(Lot lot)
        {
            Id = lot.Id;
            Name = lot.Name;
            Description = lot.Description;
            AuctionPrice = lot.AuctionPrice;
            LinkEFRSB = lot.LinkEFRSB;
            LinkTradingPlatform = lot.LinkTradingPlatform;
            MarketCost = lot.MarketCost;
            Review = lot.Review;
            InvestConcepts = lot.InvestConcepts;
        }

        public CalcViewModel(int id, string name, string description, float auctionPrice, string linkEFRSB, string linkTradingPlatform, float marketCost, string review, 
            int ic1_Id, int ic1_Period, float ic1_PurchasePrice, float ic1_SalePrice,
            int ic2_Id, int ic2_Period, float ic2_PurchasePrice, float ic2_SalePrice )
        {
            Id = id;
            Name = name;
            Description = description;
            AuctionPrice = auctionPrice;
            LinkEFRSB = linkEFRSB;
            LinkTradingPlatform = linkTradingPlatform;
            MarketCost = marketCost;
            Review = review;

            InvestConcept ic1 = new InvestConcept();
            ic1.LotId = id;
            ic1.Id = ic1_Id;
            ic1.ImplementationPeriod = ic1_Period;
            ic1.PurchasePrice = ic1_PurchasePrice;
            ic1.SalePrice = ic1_SalePrice;

            InvestConcept ic2 = new InvestConcept();
            ic2.LotId = id;
            ic2.Id = ic2_Id;
            ic2.ImplementationPeriod = ic2_Period;
            ic2.PurchasePrice = ic2_PurchasePrice;
            ic2.SalePrice = ic2_SalePrice;

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float AuctionPrice { get; set; }
        public string LinkEFRSB { get; set; }
        public string LinkTradingPlatform { get; set; }

        public float MarketCost { get; set; }
        public string Review { get; set; }

        public List<InvestConcept> InvestConcepts { get; set; } = new List<InvestConcept>();

        public float PaymentPerMonth(int icId)
        {
            try
            {
                float sum = 0;
                foreach (var exp in InvestConcepts[icId].Expenses)
                {
                    sum += exp.MonthlyPayment;
                }
                return sum;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double Income(int icId)
        {
            return Math.Round(
                InvestConcepts[icId].SalePrice - InvestConcepts[icId].PurchasePrice,
                2);
        }

        public double Profit(int icId)
        {
            return Math.Round(
                Income(icId) - (PaymentPerMonth(icId) * InvestConcepts[icId].ImplementationPeriod),
                2);
        }

        public float TotalExpenses(int icId)
        {
            return InvestConcepts[icId].PurchasePrice - (PaymentPerMonth(icId) * InvestConcepts[icId].ImplementationPeriod);
        }

        public double ProfitProc(int icId)
        {
            return Math.Round(
                Profit(icId) / TotalExpenses(icId) * 100,
                2);
        }

        public double AnnualProfitProc(int icId)
        {
            return ProfitProc(icId) * 12;
        }

    }
}
