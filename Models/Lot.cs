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
        public string Information { get; set; }
        public string Description { get; set; }

        public float AuctionPrice { get; set; }
        public float MarketCost { get; set; }

        public string LinkEFRSB { get; set; }
        public string LinkTradingPlatform { get; set; }
        public string LinkDocuments { get; set; }
        public string LinkPhotos { get; set; }

        public List<InvestConcept> InvestConcepts { get; set; } = new List<InvestConcept>();
    }
}
