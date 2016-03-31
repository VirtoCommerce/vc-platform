namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactoring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Note", "MemberId", "dbo.Member");
            AddForeignKey("dbo.Note", "MemberId", "dbo.Member", "Id", cascadeDelete: true);
            DropColumn("dbo.Organization", "Discriminator");
            DropColumn("dbo.Address", "Discriminator");
            DropColumn("dbo.Email", "Discriminator");
            DropColumn("dbo.MemberRelation", "Discriminator");
            DropColumn("dbo.Note", "Discriminator");
            DropColumn("dbo.Phone", "Discriminator");

            Sql(@"UPDATE PlatformAccount SET PlatformAccount.MemberId = M.Id FROM  Member AS M WHERE PlatformAccount.MemberId IS NULL AND PlatformAccount.Id = M.Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Phone", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Note", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.MemberRelation", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Email", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Address", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Organization", "Discriminator", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Note", "MemberId", "dbo.Member");
            AddForeignKey("dbo.Note", "MemberId", "dbo.Member", "Id");
        }
    }
}
