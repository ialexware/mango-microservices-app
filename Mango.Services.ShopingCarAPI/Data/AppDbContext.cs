using Mango.Services.ShopingCarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShopingCarAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CartHeader> Headers { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

    }

}
