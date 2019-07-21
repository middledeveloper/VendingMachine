namespace VendingMachine.Migrations
{
    using System.Data.Entity.Migrations;
    using VendingMachine.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VendingMachine.EntityConfigurations.MachineContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VendingMachine.EntityConfigurations.MachineContext ctx)
        {
            ctx.Machine.AddOrUpdate(
                new Machine() { ClientBalance = 50, TotalBalance = 50 }
            );

            ctx.SaveChanges();

            ctx.Coins.AddOrUpdate(
                new Coin() { Value = 1, Blocked = false, TotalCount = 18, MachineId = 1 },
                new Coin() { Value = 2, Blocked = false, TotalCount = 8, MachineId = 1 },
                new Coin() { Value = 5, Blocked = false, TotalCount = 13, MachineId = 1 },
                new Coin() { Value = 10, Blocked = false, TotalCount = 37, MachineId = 1 }
            );

            ctx.Products.AddOrUpdate(
                new Product() { Name = "Grape", Image = "~/Logos/grape.png", Price = 28, Storage = 20, MachineId = 1 },
                new Product() { Name = "Orange", Image = "~/Logos/orange.png", Price = 20, Storage = 20, MachineId = 1 },
                new Product() { Name = "Lemon", Image = "~/Logos/lemon.png", Price = 25, Storage = 20, MachineId = 1 },
                new Product() { Name = "Strawberry", Image = "~/Logos/strawberry.png", Price = 22, Storage = 20, MachineId = 1 }
            );
        }
    }
}