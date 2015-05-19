namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemParentRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "ParentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Item", "ParentId");
            AddForeignKey("dbo.Item", "ParentId", "dbo.Item", "Id");
			Sql("UPDATE dbo.Item  SET dbo.Item.ParentId = R.ParentItemId FROM dbo.ItemRelation as R where dbo.Item.Id = R.ChildItemId and R.[RelationTypeId] = 'Sku'"); 

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "ParentId", "dbo.Item");
            DropIndex("dbo.Item", new[] { "ParentId" });
            DropColumn("dbo.Item", "ParentId");
        }
    }
}
