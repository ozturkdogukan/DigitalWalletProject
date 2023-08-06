using Digital.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Digital.Data.Context
{
    public class DigitalEfDbContext : DbContext
    {
        public DigitalEfDbContext(DbContextOptions<DigitalEfDbContext> options) : base(options)
        {

        }

        // DbSet

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategoryMap> ProductCategoryMap { get; set; }


        //Configration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductCategoryMapConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
