namespace KBCC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerCareTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Location = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Depots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Location = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Process = c.String(),
                        Type = c.String(),
                        Model = c.String(),
                        Name = c.String(),
                        Code = c.String(),
                        BQMSCode = c.String(),
                        IMKCode = c.String(),
                        RegistrationTime = c.DateTime(),
                        Vendor = c.String(),
                        LastVersion = c.String(),
                        VersionChangingDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        Image = c.String(),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                        CheckSheet = c.String(),
                        DisposedTime = c.DateTime(),
                        DeletedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Name = c.String(),
                        Code = c.String(),
                        Weight = c.Long(nullable: false),
                        Image = c.String(),
                        BidPrice = c.Long(nullable: false),
                        AskPrice = c.Long(nullable: false),
                        Shipping = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Depot = c.String(),
                        Product = c.String(),
                        NewBidPrice = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        Location = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stocks");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.Depots");
            DropTable("dbo.Customers");
        }
    }
}
