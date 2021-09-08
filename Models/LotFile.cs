using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class LotFile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public string KeyName { get; set; }

        public int LotId { get; set; }
    }
}
