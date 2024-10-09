using Microsoft.EntityFrameworkCore;
using TestOrder.Models;

namespace TestOrder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> SO_ORDER { get; set; }
        public DbSet<Item> SO_ITEM { get; set; }
        public DbSet<Customer> COM_CUSTOMER { get; set; }
    }
}
