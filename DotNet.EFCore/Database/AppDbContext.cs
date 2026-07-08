using DotNet.EFCore.Entities;
using EFCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet.EFCore.Database
{
    public class AppDbContext : EFCoreDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }
    }
}
