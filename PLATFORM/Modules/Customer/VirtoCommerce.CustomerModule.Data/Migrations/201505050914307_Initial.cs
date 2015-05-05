namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        Line1 = c.String(nullable: false, maxLength: 128),
                        Line2 = c.String(maxLength: 128),
                        City = c.String(nullable: false, maxLength: 128),
                        CountryCode = c.String(nullable: false, maxLength: 64),
                        StateProvince = c.String(maxLength: 128),
                        CountryName = c.String(nullable: false, maxLength: 128),
                        PostalCode = c.String(nullable: false, maxLength: 32),
                        RegionId = c.String(maxLength: 128),
                        RegionName = c.String(maxLength: 128),
                        Type = c.String(maxLength: 64),
                        DaytimePhoneNumber = c.String(maxLength: 64),
                        EveningPhoneNumber = c.String(maxLength: 64),
                        FaxNumber = c.String(maxLength: 64),
                        Email = c.String(maxLength: 256),
                        Organization = c.String(maxLength: 128),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        IsValidated = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MemberRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AncestorSequence = c.Int(nullable: false),
                        AncestorId = c.String(nullable: false, maxLength: 128),
                        DescendantId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.AncestorId, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.DescendantId)
                .Index(t => t.AncestorId)
                .Index(t => t.DescendantId);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(maxLength: 128),
                        ModifierName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Body = c.String(),
                        IsSticky = c.Boolean(nullable: false),
                        MemberId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Number = c.String(maxLength: 64),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.ContactPropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        ContactId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contact", t => t.ContactId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 128),
                        TimeZone = c.String(maxLength: 32),
                        DefaultLanguage = c.String(maxLength: 32),
                        BirthDate = c.DateTime(),
                        TaxpayerId = c.String(maxLength: 64),
                        PreferredDelivery = c.String(maxLength: 64),
                        PreferredCommunication = c.String(maxLength: 64),
                        Photo = c.Binary(),
                        Salutation = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        OrgType = c.Int(nullable: false),
                        Description = c.String(maxLength: 256),
                        BusinessCategory = c.String(maxLength: 64),
                        OwnerId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "Id", "dbo.Member");
            DropForeignKey("dbo.Contact", "Id", "dbo.Member");
            DropForeignKey("dbo.ContactPropertyValue", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.Phone", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Note", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberRelation", "DescendantId", "dbo.Member");
            DropForeignKey("dbo.MemberRelation", "AncestorId", "dbo.Member");
            DropForeignKey("dbo.Email", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Address", "MemberId", "dbo.Member");
            DropIndex("dbo.Organization", new[] { "Id" });
            DropIndex("dbo.Contact", new[] { "Id" });
            DropIndex("dbo.ContactPropertyValue", new[] { "ContactId" });
            DropIndex("dbo.Phone", new[] { "MemberId" });
            DropIndex("dbo.Note", new[] { "MemberId" });
            DropIndex("dbo.MemberRelation", new[] { "DescendantId" });
            DropIndex("dbo.MemberRelation", new[] { "AncestorId" });
            DropIndex("dbo.Email", new[] { "MemberId" });
            DropIndex("dbo.Address", new[] { "MemberId" });
            DropTable("dbo.Organization");
            DropTable("dbo.Contact");
            DropTable("dbo.ContactPropertyValue");
            DropTable("dbo.Phone");
            DropTable("dbo.Note");
            DropTable("dbo.MemberRelation");
            DropTable("dbo.Email");
            DropTable("dbo.Address");
            DropTable("dbo.Member");
        }
    }
}
