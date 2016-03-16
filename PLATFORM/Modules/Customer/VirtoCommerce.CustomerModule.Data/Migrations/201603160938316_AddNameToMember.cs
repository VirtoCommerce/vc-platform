namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "Name", c => c.String(maxLength: 128));
            Sql(@"UPDATE dbo.Member SET Name = O.Name FROM dbo.Organization as O WHERE O.Id = dbo.Member.Id");
            Sql(@"UPDATE dbo.Member SET Name = C.FullName FROM dbo.Contact as C WHERE C.Id = dbo.Member.Id");
            DropColumn("dbo.Organization", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organization", "Name", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Member", "Name");
        }
    }
}
