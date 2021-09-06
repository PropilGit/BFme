using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class InvestСoncept
    {
        [Key]
        public int Id { get; set; }
        public int ImplementationPeriod { get; private set; }
        public float PurchasePrice { get; private set; }
        public float SalePrice { get; private set; }

        public List<Expense> Expenses { get; private set; }

        public string Description() 
        {
            return "Цена покупки, руб: " + PurchasePrice +
                        "Цена продажи, руб " + SalePrice;
        }
    }
}
