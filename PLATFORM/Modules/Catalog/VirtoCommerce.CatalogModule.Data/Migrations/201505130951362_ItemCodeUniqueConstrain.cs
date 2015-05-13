namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemCodeUniqueConstrain : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Item", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Item", new[] { "Code" });
        }
    }
}
