
using Machine.Data.Links;
using Machine.Data.MachineElements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Data
{
    public class MachineContext : DbContext
    {
        public DbSet<MachineElement> MachineElements { get; set; }
        public DbSet<ColliderElement> ColliderElements { get; set; }
        public DbSet<InjectorElement> InjectorElements { get; set; }
        public DbSet<InserterElement> InserterElements { get; set; }
        public DbSet<PanelHolderElement> PanelHolderElements { get; set; }
        public DbSet<ToolholderElement> ToolholderElements { get; set; }
        public DbSet<RootElement> RootElements { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<LinearLink> LinearLinks { get; set; }
        public DbSet<PneumaticLink> PneumaticLinks { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<Vector> Vectors { get; set; }
        public DbSet<Point> Points { get; set; }

        public MachineContext(DbContextOptions<MachineContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=machines.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        public async Task RemoveElement(int id)
        {
            var m = MachineElements.FirstOrDefault(e => e.MachineElementID == id);

            if (m != null)
            {
                RemoveElement(m);
                await SaveChangesAsync();
            }
        }

        private void RemoveElement(MachineElement m)
        {
            foreach (var item in m.Children)
            {
                RemoveElement(item);
            }

            RemoveElementData(m);
            MachineElements.Remove(m);
        }

        private void RemoveElementData(MachineElement m)
        {
            if (m.Color != null) Colors.Remove(m.Color);
            if (m.Transformation != null) Matrices.Remove(m.Transformation);
            if (m.LinkToParent != null) Links.Remove(m.LinkToParent);

            if (m is ColliderElement c) RemoveElementData(c);
            //else if (m is InserterElement ins) RemoveElementData(ins);
            else if (m is InjectorElement inj) RemoveElementData(inj);
            else if (m is PanelHolderElement p) RemoveElementData(p);
            else if (m is ToolholderElement t) RemoveElementData(t);
        }

        private void RemoveElementData(ColliderElement m)
        {
            Points.RemoveRange(m.Points);
        }

        //private void RemoveElementData(InserterElement m)
        //{
        //}

        private void RemoveElementData(InjectorElement m)
        {
            Vectors.Remove(m.Direction);
            Points.Remove(m.Position);
            Colors.Remove(m.InserterColor);
        }

        private void RemoveElementData(PanelHolderElement m)
        {
            Points.Remove(m.Position);
        }

        private void RemoveElementData(ToolholderElement m)
        {
            Vectors.Remove(m.Direction);
            Points.Remove(m.Position);
        }
    }
}
