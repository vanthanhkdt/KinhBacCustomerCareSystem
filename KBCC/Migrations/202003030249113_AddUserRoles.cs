namespace KBCC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                        ReturnType = c.String(),
                        Anonymouse = c.String(),
                        UserId = c.String(),
                        UserName = c.String(),
                        Permission = c.String(),
                        RouteData = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        Menu = c.String(),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        RoleId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRoles");
            DropTable("dbo.RoleActions");
            DropTable("dbo.Roles");
            DropTable("dbo.ActionHistories");
        }
    }
}
