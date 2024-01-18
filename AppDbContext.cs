using AAM.DataMaster.Model;
using Microsoft.EntityFrameworkCore;

namespace AAM.DataMaster
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
