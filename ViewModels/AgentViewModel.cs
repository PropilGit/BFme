using BFme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.ViewModels
{
    public class AgentViewModel
    {   
        public AgentViewModel(Lot Lot, ActionsEnum Action, string Message = "")
        {
            Id = Lot.Id;
            Name = Lot.Name;
            Description = Lot.Description;
            AuctionPrice = Lot.AuctionPrice;
            LinkEFRSB = Lot.LinkEFRSB;
            LinkTradingPlatform = Lot.LinkTradingPlatform;

            MarketCost = Lot.MarketCost;
            Review = Lot.Review;

            InvestConcepts = Lot.InvestConcepts;
            Files = Lot.Files;

            this.Action = Action;
            this.Message = Message;
        }

        /*
        public AgentViewModel(int Id, string Name, string Description, float AuctionPrice, string LinkEFRSB, string LinkTradingPlatform)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.AuctionPrice = AuctionPrice;
            this.LinkEFRSB = LinkEFRSB;
            this.LinkTradingPlatform = LinkTradingPlatform;

            InvestConcepts = new List<InvestConcept>();
            Files = new List<LotFile>();
            Action = ActionsEnum.Edit;
            Message = "";
        }
        */
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float AuctionPrice { get; private set; }
        public string LinkEFRSB { get; private set; }
        public string LinkTradingPlatform { get; private set; }

        public float MarketCost { get; private set; }
        public string Review { get; private set; }

        public List<InvestConcept> InvestConcepts { get; private set; } = new List<InvestConcept>();
        public List<LotFile> Files { get; private set; } = new List<LotFile>();

        public ActionsEnum Action { get; private set; }
        public string Message { get; private set; }
    }

    public enum ActionsEnum
    {
        Add,
        Edit
    }
}
