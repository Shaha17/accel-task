using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Link> Links { get; set; }
    }
}