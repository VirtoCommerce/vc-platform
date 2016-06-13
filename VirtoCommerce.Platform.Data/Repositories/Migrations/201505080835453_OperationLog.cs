namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperationLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformOperationLog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ObjectType = c.String(maxLength: 50),
                        ObjectId = c.String(maxLength: 200),
                        OperationType = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlatformOperationLog");
        }
    }
}
