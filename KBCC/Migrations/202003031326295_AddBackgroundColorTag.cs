namespace KBCC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBackgroundColorTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomId = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Icon = c.String(),
                        BackgroundColor = c.String(),
                        Color = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedIP = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "Customer", c => c.String());
            AddColumn("dbo.Orders", "CustomerName", c => c.String());
            AddColumn("dbo.Orders", "CustomerPhone", c => c.String());
            AddColumn("dbo.Orders", "CustomerAddress", c => c.String());
            AddColumn("dbo.Orders", "CustomerLocation", c => c.String());
            AddColumn("dbo.Orders", "Tags", c => c.String());
            AddColumn("dbo.Orders", "InternalNote", c => c.String());
            AddColumn("dbo.Orders", "Product", c => c.String());
            AddColumn("dbo.Orders", "TotalPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "CODPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "TransportCompany", c => c.String());
            DropColumn("dbo.Orders", "Process");
            DropColumn("dbo.Orders", "Type");
            DropColumn("dbo.Orders", "Model");
            DropColumn("dbo.Orders", "Name");
            DropColumn("dbo.Orders", "BQMSCode");
            DropColumn("dbo.Orders", "IMKCode");
            DropColumn("dbo.Orders", "RegistrationTime");
            DropColumn("dbo.Orders", "Vendor");
            DropColumn("dbo.Orders", "LastVersion");
            DropColumn("dbo.Orders", "VersionChangingDate");
            DropColumn("dbo.Orders", "Remark");
            DropColumn("dbo.Orders", "Image");
            DropColumn("dbo.Orders", "CheckSheet");
            DropColumn("dbo.Orders", "DisposedTime");
            DropColumn("dbo.Orders", "DeletedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "DeletedTime", c => c.DateTime());
            AddColumn("dbo.Orders", "DisposedTime", c => c.DateTime());
            AddColumn("dbo.Orders", "CheckSheet", c => c.String());
            AddColumn("dbo.Orders", "Image", c => c.String());
            AddColumn("dbo.Orders", "Remark", c => c.String());
            AddColumn("dbo.Orders", "VersionChangingDate", c => c.DateTime());
            AddColumn("dbo.Orders", "LastVersion", c => c.String());
            AddColumn("dbo.Orders", "Vendor", c => c.String());
            AddColumn("dbo.Orders", "RegistrationTime", c => c.DateTime());
            AddColumn("dbo.Orders", "IMKCode", c => c.String());
            AddColumn("dbo.Orders", "BQMSCode", c => c.String());
            AddColumn("dbo.Orders", "Name", c => c.String());
            AddColumn("dbo.Orders", "Model", c => c.String());
            AddColumn("dbo.Orders", "Type", c => c.String());
            AddColumn("dbo.Orders", "Process", c => c.String());
            DropColumn("dbo.Orders", "TransportCompany");
            DropColumn("dbo.Orders", "CODPrice");
            DropColumn("dbo.Orders", "TotalPrice");
            DropColumn("dbo.Orders", "Product");
            DropColumn("dbo.Orders", "InternalNote");
            DropColumn("dbo.Orders", "Tags");
            DropColumn("dbo.Orders", "CustomerLocation");
            DropColumn("dbo.Orders", "CustomerAddress");
            DropColumn("dbo.Orders", "CustomerPhone");
            DropColumn("dbo.Orders", "CustomerName");
            DropColumn("dbo.Orders", "Customer");
            DropTable("dbo.Tags");
        }
    }
}
