using System.Data.Entity;
using VendingMachine.EntityConfigurations;
using VendingMachine.Models;

namespace VendingMachine.EntityConfigurations
{
    public class MachineContext : DbContext
    {
        public MachineContext() : base("name=MachineContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Machine> Machine { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Coin> Coins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MachineConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new CoinConfiguration());

            modelBuilder.Entity<Machine>()
                .ToTable("Machine")
                .HasKey(m => m.Id);

            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Coin>()
                .ToTable("Coins")
                .HasKey(c => c.Id);
        }
    }
}