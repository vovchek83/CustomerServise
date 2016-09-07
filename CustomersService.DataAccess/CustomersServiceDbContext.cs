using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CustomersService.DataModel.Models;


namespace CustomersService.DataModel
{

    public class CustomersServiceDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    
        public DbSet<User> Users { get; set; }

        public CustomersServiceDbContext():
            base("CustomersService") 
        {
            Database.SetInitializer(new CustomersServiceInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
