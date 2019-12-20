namespace EventManagementSystem.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Tags = c.String(),
                        UploadedTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Log4NetLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Order = c.Int(nullable: false),
                        ParentModuleId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ParentModuleId)
                .Index(t => t.ParentModuleId);
            
            CreateTable(
                "dbo.ParameterGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        UserId = c.Int(),
                        ParameterGroupId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParameterGroups", t => t.ParameterGroupId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ParameterGroupId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Order = c.Int(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        CompanyName = c.String(),
                        ContractText = c.String(),
                        Adress = c.String(),
                        PhoneNumber = c.String(),
                        TCKN = c.String(),
                        PhotoPath = c.String(),
                        MailPermission = c.Boolean(nullable: false),
                        MailConfirmation = c.Boolean(nullable: false),
                        MailConfirmationCode = c.String(),
                        IsChangePassword = c.Boolean(nullable: false),
                        ChangePasswordCode = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(),
                        IsFirstLogin = c.Boolean(),
                        IsInitialized = c.Boolean(),
                        IsEmployee = c.Boolean(),
                        parentUserId = c.Int(),
                        IsTestUser = c.Boolean(nullable: false),
                        SidebarPozitionConstant = c.Boolean(nullable: false),
                        FailedLoginCount = c.Int(nullable: false),
                        SecondTitle = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ModuleId = c.Int(),
                        RoleId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ModuleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(),
                        ModuleId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.ModuleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parameters", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleModules", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleModules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.UserRoles", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Parameters", "ParameterGroupId", "dbo.ParameterGroups");
            DropForeignKey("dbo.Modules", "ParentModuleId", "dbo.Modules");
            DropIndex("dbo.RoleModules", new[] { "ModuleId" });
            DropIndex("dbo.RoleModules", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "ModuleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Parameters", new[] { "ParameterGroupId" });
            DropIndex("dbo.Parameters", new[] { "UserId" });
            DropIndex("dbo.Modules", new[] { "ParentModuleId" });
            DropTable("dbo.RoleModules");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Parameters");
            DropTable("dbo.ParameterGroups");
            DropTable("dbo.Modules");
            DropTable("dbo.Log4NetLog");
            DropTable("dbo.Files");
        }
    }
}
