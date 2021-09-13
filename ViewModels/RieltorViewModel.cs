using BFme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.ViewModels
{
    public class RieltorViewModel
    {
        public RieltorViewModel(Lot Lot, string Message = "")
        {
            Id = Lot.Id;
            Name = Lot.Name;
            MarketCost = Lot.MarketCost;
            Review = Lot.Review;
            this.Message = Message;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public float MarketCost { get; private set; }
        public string Review { get; private set; }

        public string Message { get; private set; }
    }
}
