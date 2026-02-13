#pragma warning disable 1591

namespace QuizMaker.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDisplayOrderInQuizQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "DeletedAtUtc", c => c.DateTime());
            AddColumn("dbo.Questions", "CreatedAtUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Questions", "UpdatedAtUtc", c => c.DateTime());
            AddColumn("dbo.QuizQuestions", "DisplayOrder", c => c.Int(nullable: false));
            AddColumn("dbo.QuizQuestions", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuizQuestions", "DeletedAtUtc", c => c.DateTime());
            AddColumn("dbo.QuizQuestions", "CreatedAtUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.QuizQuestions", "UpdatedAtUtc", c => c.DateTime());
            AddColumn("dbo.Quizzes", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quizzes", "DeletedAtUtc", c => c.DateTime());
            AddColumn("dbo.Quizzes", "CreatedAtUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Quizzes", "UpdatedAtUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quizzes", "UpdatedAtUtc");
            DropColumn("dbo.Quizzes", "CreatedAtUtc");
            DropColumn("dbo.Quizzes", "DeletedAtUtc");
            DropColumn("dbo.Quizzes", "IsDeleted");
            DropColumn("dbo.QuizQuestions", "UpdatedAtUtc");
            DropColumn("dbo.QuizQuestions", "CreatedAtUtc");
            DropColumn("dbo.QuizQuestions", "DeletedAtUtc");
            DropColumn("dbo.QuizQuestions", "IsDeleted");
            DropColumn("dbo.QuizQuestions", "DisplayOrder");
            DropColumn("dbo.Questions", "UpdatedAtUtc");
            DropColumn("dbo.Questions", "CreatedAtUtc");
            DropColumn("dbo.Questions", "DeletedAtUtc");
            DropColumn("dbo.Questions", "IsDeleted");
        }
    }
}
