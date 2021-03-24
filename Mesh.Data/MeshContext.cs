using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.Data
{
    public class MeshContext : DbContext
    {
        public DbSet<Mesh> Meshes { get; set; }

        public MeshContext(DbContextOptions<MeshContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Meshes.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            if (!Database.CanConnect()) Database.Migrate();

            return base.SaveChanges();
        }

        public IEnumerable<MeshInfo> GetMeshInfos()
        {
            return Meshes.Select(m => new MeshInfo()
            {
                MeshID = m.MeshID,
                Name = m.Name,
                Size = m.Data.Length
            }).ToList();
        }

        public Mesh GetMesh(int id)
        {
            return Meshes.FirstOrDefault(m => m.MeshID == id);
        }

        public async Task<MeshInfo> AddOrUpdateAsync(Mesh model, bool update = true)
        {
            var mesh = Meshes.FirstOrDefault(m => string.Compare(m.Name, model.Name) == 0);

            if (mesh != null)
            {
                if (update)
                {
                    mesh.Data = model.Data;
                    await SaveChangesAsync();
                }

                return new MeshInfo()
                {
                    MeshID = mesh.MeshID,
                    Name = mesh.Name,
                    Size = mesh.Data.Length
                };
            }
            else
            {
                Meshes.Add(model);
                await SaveChangesAsync();
                return new MeshInfo()
                {
                    MeshID = model.MeshID,
                    Name = model.Name,
                    Size = model.Data.Length
                };
            }
        }

        public async void Delete(int id)
        {
            var mesh = Meshes.FirstOrDefault(m => m.MeshID == id);

            if (mesh != null)
            {
                Meshes.Remove(mesh);
                await SaveChangesAsync();
            }
        }
    }
}
