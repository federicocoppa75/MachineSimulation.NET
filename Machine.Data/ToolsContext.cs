using Machine.Data.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data
{
    public class ToolsContext : DbContext
    {
        public DbSet<Tool> Tools { get; set; }
        public DbSet<CountersinkTool> CountersinkTools { get; set; }
        public DbSet<DiskOnConeTool> DiskOnConeTools { get; set; }
        public DbSet<DiskTool> DiskTools { get; set; }
        public DbSet<PointedTool> PointedTools { get; set; }
        public DbSet<SimpleTool> SimpleTools { get; set; }
        public DbSet<TwoSectionTool> TwoSectionTools { get; set; }
        public DbSet<ToolSet> ToolSets { get; set; }

        public ToolsContext(DbContextOptions<ToolsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tools.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
