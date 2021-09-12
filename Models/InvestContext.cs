using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Models
{
    public class InvestContext : DbContext
    {
        public DbSet<Lot> Lots { get; set; }
        public DbSet<InvestConcept> InvestConcepts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<LotFile> Files { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        public InvestContext(DbContextOptions<InvestContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
