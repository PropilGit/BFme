using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float SinglePayment { get; set; }
        public float MonthlyPayment { get; set; }
    }
}
