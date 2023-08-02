using AsynchronousProgrammingMVC.Models.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AsynchronousProgrammingMVC.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Migrations safhasında "Microsoft.EntityFrameworkCore.Model.Validation[30000]" warning'i yememek için aşağıdaki işlem yapılır.
            modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnType("decimal");


            base.OnModelCreating(modelBuilder);
        }
    }
}
