using System.Configuration;

using Microsoft.EntityFrameworkCore;

namespace DbMap.Benchmark
{
    public class DbMapDbContext : DbContext
    {
        public DbSet<Tiny> Tiny { get; set; }

        public DbSet<ExtraSmall> ExtraSmall { get; set; }

        public DbSet<Small> Small { get; set; }

        public DbSet<Medium> Medium { get; set; }

        public DbSet<Large> Large { get; set; }

        public DbSet<ExtraLarge> ExtraLarge { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tiny>().HasNoKey();
            modelBuilder.Entity<ExtraSmall>().HasNoKey();
            modelBuilder.Entity<Small>().HasNoKey();
            modelBuilder.Entity<Medium>().HasNoKey();
            modelBuilder.Entity<Large>().HasNoKey();
            modelBuilder.Entity<ExtraLarge>().HasNoKey();
        }
    }
}
