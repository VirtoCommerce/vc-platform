namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalPropertySet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Item", "PropertySetId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Item", "PropertySetId", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
