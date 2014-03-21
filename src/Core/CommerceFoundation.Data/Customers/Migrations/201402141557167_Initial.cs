using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Customers.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        ContractId = c.String(nullable: false, maxLength: 64),
                        MemberType = c.Int(nullable: false),
                        ContractState = c.Int(nullable: false),
                        ContractVersion = c.String(maxLength: 32),
                        Comments = c.String(maxLength: 512),
                        CreditAllowed = c.Boolean(nullable: false),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditLimitCurrency = c.String(maxLength: 16),
                        SpendingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ContractId)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        EmailId = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        IsValidated = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmailId)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Label",
                c => new
                    {
                        LabelId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        ImgUrl = c.String(maxLength: 256),
                        Description = c.String(nullable: false, maxLength: 512),
                        MemberId = c.String(maxLength: 128),
                        CaseId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LabelId)
                .ForeignKey("dbo.Case", t => t.CaseId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.CaseId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Case",
                c => new
                    {
                        CaseId = c.String(nullable: false, maxLength: 128),
                        Number = c.String(maxLength: 128),
                        Priority = c.Int(nullable: false),
                        AgentId = c.String(maxLength: 128),
                        AgentName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 256),
                        Description = c.String(maxLength: 512),
                        Status = c.String(maxLength: 64),
                        Channel = c.String(maxLength: 64),
                        ContactDisplayName = c.String(maxLength: 128),
                        CaseTemplateId = c.String(maxLength: 64),
                        ContactId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseId)
                .ForeignKey("dbo.CaseTemplate", t => t.CaseTemplateId)
                .ForeignKey("dbo.Contact", t => t.ContactId)
                .Index(t => t.CaseTemplateId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.CaseCC",
                c => new
                    {
                        CaseCCId = c.String(nullable: false, maxLength: 64),
                        CaseId = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseCCId)
                .ForeignKey("dbo.Case", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.CasePropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
                        CaseId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.Case", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.CaseTemplate",
                c => new
                    {
                        CaseTemplateId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseTemplateId);
            
            CreateTable(
                "dbo.CaseTemplateProperty",
                c => new
                    {
                        CaseTemplatePropertyId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        CaseTemplateId = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseTemplatePropertyId)
                .ForeignKey("dbo.CaseTemplate", t => t.CaseTemplateId, cascadeDelete: true)
                .Index(t => t.CaseTemplateId);
            
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        AttachmentId = c.String(nullable: false, maxLength: 128),
                        CreatorName = c.String(maxLength: 128),
                        FileUrl = c.String(maxLength: 512),
                        DisplayName = c.String(maxLength: 128),
                        FileType = c.String(maxLength: 64),
                        CommunicationItemId = c.String(maxLength: 128),
                        KnowledgeBaseArticleId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AttachmentId)
                .ForeignKey("dbo.KnowledgeBaseArticle", t => t.KnowledgeBaseArticleId)
                .ForeignKey("dbo.CommunicationItem", t => t.CommunicationItemId)
                .Index(t => t.KnowledgeBaseArticleId)
                .Index(t => t.CommunicationItemId);
            
            CreateTable(
                "dbo.KnowledgeBaseArticle",
                c => new
                    {
                        KnowledgeBaseArticleId = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(maxLength: 128),
                        AuthorId = c.String(maxLength: 128),
                        ModifierName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Body = c.String(),
                        GroupId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.KnowledgeBaseArticleId)
                .ForeignKey("dbo.KnowledgeBaseGroup", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.KnowledgeBaseGroup",
                c => new
                    {
                        KnowledgeBaseGroupId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ParentId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.KnowledgeBaseGroupId)
                .ForeignKey("dbo.KnowledgeBaseGroup", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.CommunicationItem",
                c => new
                    {
                        CommunicationItemId = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(maxLength: 128),
                        ModifierId = c.String(maxLength: 128),
                        ModifierName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Body = c.String(),
                        CaseId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.CommunicationItemId)
                .ForeignKey("dbo.Case", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        MemberId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.MemberId);
            
            CreateTable(
                "dbo.ContactPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
                        ContactId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        NoteId = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(maxLength: 128),
                        ModifierName = c.String(maxLength: 128),
                        Title = c.String(maxLength: 128),
                        Body = c.String(),
                        IsSticky = c.Boolean(nullable: false),
                        MemberId = c.String(maxLength: 128),
                        CaseId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Case", t => t.CaseId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.CaseId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MemberRelation",
                c => new
                    {
                        MemberRelationId = c.String(nullable: false, maxLength: 128),
                        AncestorSequence = c.Int(nullable: false),
                        AncestorId = c.String(nullable: false, maxLength: 128),
                        DescendantId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MemberRelationId)
                .ForeignKey("dbo.Member", t => t.AncestorId, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.DescendantId)
                .Index(t => t.AncestorId)
                .Index(t => t.DescendantId);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        PhoneId = c.String(nullable: false, maxLength: 128),
                        Number = c.String(maxLength: 64),
                        Type = c.String(maxLength: 64),
                        MemberId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PhoneId)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.CasePropertySet",
                c => new
                    {
                        CasePropertySetId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CasePropertySetId);
            
            CreateTable(
                "dbo.CaseProperty",
                c => new
                    {
                        CasePropertyId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        FieldName = c.String(maxLength: 128),
                        CasePropertySetId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CasePropertyId)
                .ForeignKey("dbo.CasePropertySet", t => t.CasePropertySetId, cascadeDelete: true)
                .Index(t => t.CasePropertySetId);
            
            CreateTable(
                "dbo.CaseRule",
                c => new
                    {
                        CaseRuleId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Status = c.Int(nullable: false),
                        PredicateSerialized = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseRuleId);
            
            CreateTable(
                "dbo.CaseAlert",
                c => new
                    {
                        CaseAlertId = c.String(nullable: false, maxLength: 128),
                        XslTemplate = c.String(),
                        HtmlBody = c.String(),
                        RedirectUrl = c.String(maxLength: 512),
                        CaseRuleId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CaseAlertId)
                .ForeignKey("dbo.CaseRule", t => t.CaseRuleId, cascadeDelete: true)
                .Index(t => t.CaseRuleId);
            
            CreateTable(
                "dbo.Contact",
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
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.PhoneCallItem",
                c => new
                    {
                        CommunicationItemId = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(maxLength: 64),
                        Direction = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.CommunicationItemId)
                .ForeignKey("dbo.CommunicationItem", t => t.CommunicationItemId)
                .Index(t => t.CommunicationItemId);
            
            CreateTable(
                "dbo.EmailItem",
                c => new
                    {
                        CommunicationItemId = c.String(nullable: false, maxLength: 128),
                        From = c.String(maxLength: 128),
                        To = c.String(maxLength: 128),
                        Subject = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.CommunicationItemId)
                .ForeignKey("dbo.CommunicationItem", t => t.CommunicationItemId)
                .Index(t => t.CommunicationItemId);
            
            CreateTable(
                "dbo.PublicReplyItem",
                c => new
                    {
                        CommunicationItemId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CommunicationItemId)
                .ForeignKey("dbo.CommunicationItem", t => t.CommunicationItemId)
                .Index(t => t.CommunicationItemId);
            
            CreateTable(
                "dbo.Organization",
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
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "MemberId", "dbo.Member");
            DropForeignKey("dbo.PublicReplyItem", "CommunicationItemId", "dbo.CommunicationItem");
            DropForeignKey("dbo.EmailItem", "CommunicationItemId", "dbo.CommunicationItem");
            DropForeignKey("dbo.PhoneCallItem", "CommunicationItemId", "dbo.CommunicationItem");
            DropForeignKey("dbo.Contact", "MemberId", "dbo.Member");
            DropForeignKey("dbo.CaseAlert", "CaseRuleId", "dbo.CaseRule");
            DropForeignKey("dbo.CaseProperty", "CasePropertySetId", "dbo.CasePropertySet");
            DropForeignKey("dbo.Phone", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Note", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberRelation", "DescendantId", "dbo.Member");
            DropForeignKey("dbo.MemberRelation", "AncestorId", "dbo.Member");
            DropForeignKey("dbo.Label", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Note", "CaseId", "dbo.Case");
            DropForeignKey("dbo.Label", "CaseId", "dbo.Case");
            DropForeignKey("dbo.ContactPropertyValue", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.Case", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.CommunicationItem", "CaseId", "dbo.Case");
            DropForeignKey("dbo.Attachment", "CommunicationItemId", "dbo.CommunicationItem");
            DropForeignKey("dbo.KnowledgeBaseGroup", "ParentId", "dbo.KnowledgeBaseGroup");
            DropForeignKey("dbo.KnowledgeBaseArticle", "GroupId", "dbo.KnowledgeBaseGroup");
            DropForeignKey("dbo.Attachment", "KnowledgeBaseArticleId", "dbo.KnowledgeBaseArticle");
            DropForeignKey("dbo.Case", "CaseTemplateId", "dbo.CaseTemplate");
            DropForeignKey("dbo.CaseTemplateProperty", "CaseTemplateId", "dbo.CaseTemplate");
            DropForeignKey("dbo.CasePropertyValue", "CaseId", "dbo.Case");
            DropForeignKey("dbo.CaseCC", "CaseId", "dbo.Case");
            DropForeignKey("dbo.Email", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Contract", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Address", "MemberId", "dbo.Member");
            DropIndex("dbo.Organization", new[] { "MemberId" });
            DropIndex("dbo.PublicReplyItem", new[] { "CommunicationItemId" });
            DropIndex("dbo.EmailItem", new[] { "CommunicationItemId" });
            DropIndex("dbo.PhoneCallItem", new[] { "CommunicationItemId" });
            DropIndex("dbo.Contact", new[] { "MemberId" });
            DropIndex("dbo.CaseAlert", new[] { "CaseRuleId" });
            DropIndex("dbo.CaseProperty", new[] { "CasePropertySetId" });
            DropIndex("dbo.Phone", new[] { "MemberId" });
            DropIndex("dbo.Note", new[] { "MemberId" });
            DropIndex("dbo.MemberRelation", new[] { "DescendantId" });
            DropIndex("dbo.MemberRelation", new[] { "AncestorId" });
            DropIndex("dbo.Label", new[] { "MemberId" });
            DropIndex("dbo.Note", new[] { "CaseId" });
            DropIndex("dbo.Label", new[] { "CaseId" });
            DropIndex("dbo.ContactPropertyValue", new[] { "ContactId" });
            DropIndex("dbo.Case", new[] { "ContactId" });
            DropIndex("dbo.CommunicationItem", new[] { "CaseId" });
            DropIndex("dbo.Attachment", new[] { "CommunicationItemId" });
            DropIndex("dbo.KnowledgeBaseGroup", new[] { "ParentId" });
            DropIndex("dbo.KnowledgeBaseArticle", new[] { "GroupId" });
            DropIndex("dbo.Attachment", new[] { "KnowledgeBaseArticleId" });
            DropIndex("dbo.Case", new[] { "CaseTemplateId" });
            DropIndex("dbo.CaseTemplateProperty", new[] { "CaseTemplateId" });
            DropIndex("dbo.CasePropertyValue", new[] { "CaseId" });
            DropIndex("dbo.CaseCC", new[] { "CaseId" });
            DropIndex("dbo.Email", new[] { "MemberId" });
            DropIndex("dbo.Contract", new[] { "MemberId" });
            DropIndex("dbo.Address", new[] { "MemberId" });
            DropTable("dbo.Organization");
            DropTable("dbo.PublicReplyItem");
            DropTable("dbo.EmailItem");
            DropTable("dbo.PhoneCallItem");
            DropTable("dbo.Contact");
            DropTable("dbo.CaseAlert");
            DropTable("dbo.CaseRule");
            DropTable("dbo.CaseProperty");
            DropTable("dbo.CasePropertySet");
            DropTable("dbo.Phone");
            DropTable("dbo.MemberRelation");
            DropTable("dbo.Note");
            DropTable("dbo.ContactPropertyValue");
            DropTable("dbo.Member");
            DropTable("dbo.CommunicationItem");
            DropTable("dbo.KnowledgeBaseGroup");
            DropTable("dbo.KnowledgeBaseArticle");
            DropTable("dbo.Attachment");
            DropTable("dbo.CaseTemplateProperty");
            DropTable("dbo.CaseTemplate");
            DropTable("dbo.CasePropertyValue");
            DropTable("dbo.CaseCC");
            DropTable("dbo.Case");
            DropTable("dbo.Label");
            DropTable("dbo.Email");
            DropTable("dbo.Contract");
            DropTable("dbo.Address");
        }
    }
}
