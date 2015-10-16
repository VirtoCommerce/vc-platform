namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryIdToItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "CategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Item", "CategoryId");
            AddForeignKey("dbo.Item", "CategoryId", "dbo.Category", "Id");
			Sql("UPDATE Item SET  Item.CategoryId = CIR.CategoryId FROM  Item  INNER JOIN  CategoryItemRelation CIR ON Item.Id = CIR.ItemId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "CategoryId", "dbo.Category");
            DropIndex("dbo.Item", new[] { "CategoryId" });
            DropColumn("dbo.Item", "CategoryId");
        }
    }
}
