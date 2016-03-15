namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMemberTypeToMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "MemberType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "MemberType");
        }
    }
}
