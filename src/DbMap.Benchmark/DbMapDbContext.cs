using System.Configuration;

using Microsoft.EntityFrameworkCore;

namespace DbMap.Benchmark
{
    public class DbMapDbContext : DbContext
    {
        public DbSet<Small> Small { get; set; }

        public DbSet<Medium> Medium { get; set; }

        public DbSet<Large> Large { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Small>().HasNoKey();
            modelBuilder.Entity<Medium>().HasNoKey();
            modelBuilder.Entity<Large>().HasNoKey();
        }
    }
}
