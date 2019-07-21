namespace VendingMachine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCount = c.Int(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        MachineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Machine", t => t.MachineId, cascadeDelete: false)
                .Index(t => t.MachineId);
            
            CreateTable(
                "dbo.Machine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Image = c.String(nullable: false, maxLength: 255),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Storage = c.Int(nullable: false),
                        MachineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Machine", t => t.MachineId, cascadeDelete: false)
                .Index(t => t.MachineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "MachineId", "dbo.Machine");
            DropForeignKey("dbo.Coins", "MachineId", "dbo.Machine");
            DropIndex("dbo.Products", new[] { "MachineId" });
            DropIndex("dbo.Coins", new[] { "MachineId" });
            DropTable("dbo.Products");
            DropTable("dbo.Machine");
            DropTable("dbo.Coins");
        }
    }
}
