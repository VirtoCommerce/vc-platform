namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.vc_Address",
                c => new
                    {
                        AddressId = c.String(nullable: false, maxLength: 128),
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
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_Member",
                c => new
                    {
                        MemberId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_Email",
                c => new
                    {
                        EmailId = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        IsValidated = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmailId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_MemberRelation",
                c => new
                    {
                        MemberRelationId = c.String(nullable: false, maxLength: 128),
                        AncestorSequence = c.Int(nullable: false),
                        AncestorId = c.String(nullable: false, maxLength: 128),
                        DescendantId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MemberRelationId)
                .ForeignKey("dbo.vc_Member", t => t.AncestorId, cascadeDelete: true)
                .ForeignKey("dbo.vc_Member", t => t.DescendantId)
                .Index(t => t.AncestorId)
                .Index(t => t.DescendantId);
            
            CreateTable(
                "dbo.vc_Note",
                c => new
                    {
                        NoteId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 64),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 64),
                        AuthorName = c.String(maxLength: 128),
                        ModifierName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Body = c.String(),
                        IsSticky = c.Boolean(nullable: false),
                        MemberId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_Phone",
                c => new
                    {
                        PhoneId = c.String(nullable: false, maxLength: 128),
                        Number = c.String(maxLength: 64),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PhoneId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_ContactPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.vc_Contact", t => t.ContactId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.vc_Contact",
                c => new
                    {
                        MemberId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.MemberId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.vc_Organization",
                c => new
                    {
                        MemberId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        OrgType = c.Int(nullable: false),
                        Description = c.String(maxLength: 256),
                        BusinessCategory = c.String(maxLength: 64),
                        OwnerId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MemberId)
                .ForeignKey("dbo.vc_Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.vc_Organization", "MemberId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_Contact", "MemberId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_ContactPropertyValue", "ContactId", "dbo.vc_Contact");
            DropForeignKey("dbo.vc_Phone", "MemberId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_Note", "MemberId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_MemberRelation", "DescendantId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_MemberRelation", "AncestorId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_Email", "MemberId", "dbo.vc_Member");
            DropForeignKey("dbo.vc_Address", "MemberId", "dbo.vc_Member");
            DropIndex("dbo.vc_Organization", new[] { "MemberId" });
            DropIndex("dbo.vc_Contact", new[] { "MemberId" });
            DropIndex("dbo.vc_ContactPropertyValue", new[] { "ContactId" });
            DropIndex("dbo.vc_Phone", new[] { "MemberId" });
            DropIndex("dbo.vc_Note", new[] { "MemberId" });
            DropIndex("dbo.vc_MemberRelation", new[] { "DescendantId" });
            DropIndex("dbo.vc_MemberRelation", new[] { "AncestorId" });
            DropIndex("dbo.vc_Email", new[] { "MemberId" });
            DropIndex("dbo.vc_Address", new[] { "MemberId" });
            DropTable("dbo.vc_Organization");
            DropTable("dbo.vc_Contact");
            DropTable("dbo.vc_ContactPropertyValue");
            DropTable("dbo.vc_Phone");
            DropTable("dbo.vc_Note");
            DropTable("dbo.vc_MemberRelation");
            DropTable("dbo.vc_Email");
            DropTable("dbo.vc_Member");
            DropTable("dbo.vc_Address");
        }
    }
}
