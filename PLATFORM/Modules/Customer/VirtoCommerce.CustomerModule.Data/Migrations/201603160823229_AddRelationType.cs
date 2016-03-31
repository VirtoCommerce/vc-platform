namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberRelation", "RelationType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberRelation", "RelationType");
        }
    }
}
