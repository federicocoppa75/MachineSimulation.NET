using Machine.Data.Toolings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data
{
    public class ToolingContext : DbContext
    {
        public DbSet<ToolingUnit> ToolingUnits { get; set; }
        public DbSet<Tooling> Toolings { get; set; }

        public ToolingContext(DbContextOptions<ToolingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tooling.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
