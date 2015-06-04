using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Reviews.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Review",
                c => new
                    {
                        ReviewId = c.String(nullable: false, maxLength: 128),
                        ItemUrl = c.String(nullable: false, maxLength: 1024),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        SchemaId = c.Int(nullable: false),
                        Title = c.String(maxLength: 128),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        AuthorLocation = c.String(maxLength: 128),
                        OverallRating = c.Int(nullable: false),
                        TotalAbuseCount = c.Int(nullable: false),
                        TotalPositiveFeedbackCount = c.Int(nullable: false),
                        TotalNegativeFeedbackCount = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ContentLocale = c.String(maxLength: 64),
                        IsVerifiedBuyer = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReviewId);
            
            CreateTable(
                "dbo.MediaContent",
                c => new
                    {
                        MediaContentId = c.String(nullable: false, maxLength: 128),
                        ReviewId = c.String(nullable: false, maxLength: 128),
                        SmallUrl = c.String(maxLength: 1024),
                        LargeUrl = c.String(maxLength: 1024),
                        MediumUrl = c.String(maxLength: 1024),
                        ContentType = c.String(maxLength: 64),
                        Description = c.String(maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MediaContentId)
                .ForeignKey("dbo.Review", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.ReviewComment",
                c => new
                    {
                        ReviewCommentId = c.String(nullable: false, maxLength: 128),
                        ReviewId = c.String(nullable: false, maxLength: 128),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        AuthorLocation = c.String(maxLength: 128),
                        Comment = c.String(maxLength: 1024),
                        Status = c.Int(nullable: false),
                        TotalAbuseCount = c.Int(nullable: false),
                        TotalPositiveFeedbackCount = c.Int(nullable: false),
                        TotalNegativeFeedbackCount = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReviewCommentId)
                .ForeignKey("dbo.Review", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.ReviewFieldValue",
                c => new
                    {
                        ReviewFieldValueId = c.String(nullable: false, maxLength: 128),
                        ReviewId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Value = c.String(maxLength: 1024),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReviewFieldValueId)
                .ForeignKey("dbo.Review", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.ReportAbuseElement",
                c => new
                    {
                        ReportElementId = c.String(nullable: false, maxLength: 128),
                        Comment = c.String(maxLength: 1024),
                        Email = c.String(nullable: false, maxLength: 128),
                        Reason = c.String(nullable: false, maxLength: 16),
                        ReviewId = c.String(maxLength: 128),
                        CommentId = c.String(maxLength: 128),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReportElementId)
                .ForeignKey("dbo.Review", t => t.ReviewId)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.ReportHelpfulElement",
                c => new
                    {
                        ReportElementId = c.String(nullable: false, maxLength: 128),
                        IsHelpful = c.Boolean(nullable: false),
                        ReviewId = c.String(maxLength: 128),
                        CommentId = c.String(maxLength: 128),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReportElementId)
                .ForeignKey("dbo.Review", t => t.ReviewId)
                .Index(t => t.ReviewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportHelpfulElement", "ReviewId", "dbo.Review");
            DropForeignKey("dbo.ReportAbuseElement", "ReviewId", "dbo.Review");
            DropForeignKey("dbo.ReviewFieldValue", "ReviewId", "dbo.Review");
            DropForeignKey("dbo.ReviewComment", "ReviewId", "dbo.Review");
            DropForeignKey("dbo.MediaContent", "ReviewId", "dbo.Review");
            DropIndex("dbo.ReportHelpfulElement", new[] { "ReviewId" });
            DropIndex("dbo.ReportAbuseElement", new[] { "ReviewId" });
            DropIndex("dbo.ReviewFieldValue", new[] { "ReviewId" });
            DropIndex("dbo.ReviewComment", new[] { "ReviewId" });
            DropIndex("dbo.MediaContent", new[] { "ReviewId" });
            DropTable("dbo.ReportHelpfulElement");
            DropTable("dbo.ReportAbuseElement");
            DropTable("dbo.ReviewFieldValue");
            DropTable("dbo.ReviewComment");
            DropTable("dbo.MediaContent");
            DropTable("dbo.Review");
        }
    }
}
