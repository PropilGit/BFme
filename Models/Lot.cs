using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float AuctionPrice { get; set; }
        public string LinkEFRSB { get; set; }
        public string LinkTradingPlatform { get; set; }

        public float MarketCost { get; set; }
        public string Review { get; set; }


        //public LotReview Review { get; set; } = new LotReview();
        public List<InvestConcept> InvestConcepts { get; set; } = new List<InvestConcept>();
        public List<LotFile> Files { get; set; } = new List<LotFile>();
    }
}
