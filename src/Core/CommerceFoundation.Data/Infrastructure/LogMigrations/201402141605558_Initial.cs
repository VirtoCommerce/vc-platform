using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Infrastructure.LogMigrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.__OperationLogs",
                c => new
                    {
                        OperationLogId = c.String(nullable: false, maxLength: 128),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ObjectType = c.String(nullable: false, maxLength: 50),
                        ObjectId = c.String(nullable: false, maxLength: 200),
                        TableName = c.String(nullable: false, maxLength: 200),
                        OperationType = c.String(nullable: false, maxLength: 20),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OperationLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.__OperationLogs");
        }
    }
}
