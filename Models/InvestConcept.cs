using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class InvestConcept
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ImplementationPeriod { get; set; }
        public float PurchasePrice { get; set; }
        public float SalePrice { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public int LotId { get; set; }

        public string Description() => "";
    }
}
