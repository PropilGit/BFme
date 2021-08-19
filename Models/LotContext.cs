using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class LotContext : DbContext
    {
        public DbSet<Lot> Lots { get; set; }
        public LotContext(DbContextOptions<LotContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
