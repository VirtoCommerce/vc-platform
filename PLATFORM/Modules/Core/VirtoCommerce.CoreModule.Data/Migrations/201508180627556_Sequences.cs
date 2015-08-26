namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sequences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sequence",
                c => new
                    {
                        ObjectType = c.String(nullable: false, maxLength: 256),
                        Value = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Id = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ObjectType);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sequence");
        }
    }
}
