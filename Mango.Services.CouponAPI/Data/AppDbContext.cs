using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon { CouponId = 1, CouponCode = "10OFF", DsicountAmount = 10, MinAmount = 100 });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { CouponId = 2, CouponCode = "20OFF", DsicountAmount = 20, MinAmount = 200 });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { CouponId = 3, CouponCode = "30OFF", DsicountAmount = 30, MinAmount = 300 });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { CouponId = 4, CouponCode = "40OFF", DsicountAmount = 40, MinAmount = 400 });
            modelBuilder.Entity<Coupon>().HasData(new Coupon { CouponId = 5, CouponCode = "50OFF", DsicountAmount = 50, MinAmount = 500 });
        }   

    }

}
