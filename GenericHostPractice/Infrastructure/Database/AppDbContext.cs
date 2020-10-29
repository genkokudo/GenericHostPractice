using GenericHostPractice.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenericHostPractice.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestEntity> Tests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
    }
}
