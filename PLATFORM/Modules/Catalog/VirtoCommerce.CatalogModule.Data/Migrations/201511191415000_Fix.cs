namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE dbo.Property SET CatalogId = C.CatalogId FROM Category as C            
                  WHERE C.Id = dbo.Property.CategoryId");
        }

        public override void Down()
        {
        }
    }
}
