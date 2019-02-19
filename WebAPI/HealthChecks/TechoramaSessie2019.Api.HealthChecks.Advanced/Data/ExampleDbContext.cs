using Microsoft.EntityFrameworkCore;

namespace TechoramaSessie2019.Api.HealthChecks.Advanced.Data
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
