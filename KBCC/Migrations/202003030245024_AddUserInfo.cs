namespace KBCC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsApproved", c => c.Boolean());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
            AddColumn("dbo.AspNetUsers", "PathProfileImage", c => c.String());
            AddColumn("dbo.AspNetUsers", "RegistrationTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LastLoginTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LastIPAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsBlocked", c => c.Boolean());
            AddColumn("dbo.AspNetUsers", "Premises", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsPasswordExpired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsPasswordExpired");
            DropColumn("dbo.AspNetUsers", "Premises");
            DropColumn("dbo.AspNetUsers", "IsBlocked");
            DropColumn("dbo.AspNetUsers", "LastIPAddress");
            DropColumn("dbo.AspNetUsers", "LastLoginTime");
            DropColumn("dbo.AspNetUsers", "RegistrationTime");
            DropColumn("dbo.AspNetUsers", "PathProfileImage");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "IsApproved");
        }
    }
}
