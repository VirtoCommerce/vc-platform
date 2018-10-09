namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountEntityPasswordExpired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformAccount", "PasswordExpired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformAccount", "PasswordExpired");
        }
    }
}
