namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewContactFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "FirstName", c => c.String(maxLength: 128));
            AddColumn("dbo.Contact", "MiddleName", c => c.String(maxLength: 128));
            AddColumn("dbo.Contact", "LastName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Contact", "FullName", c => c.String(nullable: false, maxLength: 254));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contact", "FullName", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Contact", "LastName");
            DropColumn("dbo.Contact", "MiddleName");
            DropColumn("dbo.Contact", "FirstName");
        }
    }
}
