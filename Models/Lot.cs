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
        public float StartCost { get; set; }
        public float ProjectedCost { get; set; }
        public float MarketCost { get; set; }
        public DateTime AuctionDate { get; set; }
        public string Description { get; set; }
        public string Assessment { get; set; }
        public string Assignment { get; set; }
        public string Neighbors { get; set; }
        public string ProjectedSale { get; set; }
    }
}
