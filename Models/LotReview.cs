using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class LotReview
    {
        [Key]
        public int Id { get; set; }
        public float MarketCost { get; set; }
        public string Description { get; set; }

        public int LotId { get; set; }
    }
}
