namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentTheme",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ThemePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContentTheme");
        }
    }
}
