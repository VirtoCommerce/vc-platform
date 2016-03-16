namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMemberTypeToMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "MemberType", c => c.String(maxLength: 64));
            Sql(@"UPDATE dbo.Member SET MemberType = 'Contact' FROM dbo.Contact as C WHERE C.Id  = dbo.Member.Id");
            Sql(@"UPDATE dbo.Member SET MemberType = 'Organization' FROM dbo.Organization as O WHERE O.Id  = dbo.Member.Id");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "MemberType");
        }
    }
}
